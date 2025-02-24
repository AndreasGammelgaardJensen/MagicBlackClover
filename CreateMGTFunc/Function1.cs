using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DataAccess;
using MagicScannerLib.Messaging;
using MagicScannerLib.Messaging.AzureServiceBus;
using MagicScannerLib.Models;
using MagicScannerLib.Models.Database;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using static MagicScannerLib.Models.ACVOCRResultModel;

namespace CreateMGTFunc
{
	public class Function1
	{
		private readonly ILogger<Function1> _logger;
		private readonly IDbContextFactory<DataContext> _contextFactory;
		private readonly ServiceBusHandler _serviceBusHandler;


		public Function1(ILogger<Function1> logger, IDbContextFactory<DataContext> contextFactory, ServiceBusHandler serviceBusHandler)
		{
			_logger = logger;
			_contextFactory = contextFactory;
			_serviceBusHandler = serviceBusHandler;
		}

		[Function(nameof(Function1))]
		public async Task Run(
			[ServiceBusTrigger(MagicQueue.createQueue, Connection = "ServiceBusSettings:ConnectionString")]
			ServiceBusReceivedMessage message,
			ServiceBusMessageActions messageActions)
		{
			_logger.LogInformation("Message ID: {id}", message.MessageId);
			_logger.LogInformation("Message Body: {body}", message.Body);
			_logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

			var messageModel = JsonSerializer.Deserialize<CVCreateMessageModel>(message.Body);
			messageModel.TimesProcessed++;

			if (messageModel.TimesProcessed > 2)
				throw new Exception("This cannot be processed");

			//Load to database
			var dbcontext = await _contextFactory.CreateDbContextAsync();

			var card = dbcontext.Cards.Where(c => c.Name == messageModel.Name).FirstOrDefault();

			//If exist in the database add it to the user collection
			if (card != null)
			{
				dbcontext.CollectionCards.Add(new CollectionCardsDatabaseModel
				{
					CardId = card.Id,
					CollectionId = messageModel.CollectionId
				});

				var scanItemDbModel = dbcontext.ScanItems.Single(s => s.Id == messageModel.ScanItemId);
				scanItemDbModel.CardId = card.Id;
				dbcontext.ScanItems.Update(scanItemDbModel);

				dbcontext.SaveChanges();
			}
			else if (card == null && messageModel.TimesProcessed == 2)
			{
				throw new Exception($"This cannot be processed {JsonSerializer.Serialize(messageModel)}");
			}
			else
			{
				await _serviceBusHandler.SendMessageAsync(JsonSerializer.Serialize(messageModel), MagicQueue.downloadQueue);
			}

			//TODO: Check if the card is already in in the queue

			// Complete the message
			await messageActions.CompleteMessageAsync(message);
		}
	}
}
