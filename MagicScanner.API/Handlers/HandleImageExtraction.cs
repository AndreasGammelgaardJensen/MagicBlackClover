using DataAccess;
using MagicScannerLib.ComputerVision;
using MagicScannerLib.Messaging;
using MagicScannerLib.Messaging.AzureServiceBus;
using MagicScannerLib.Models;
using MagicScannerLib.Models.Database;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MagicScanner.API.Handlers
{
	public class HandleImageExtraction
	{

		private readonly ServiceBusHandler _serviceBusHandler;
		private readonly DataContext _dbcontext;


		public HandleImageExtraction(ServiceBusHandler serviceBusHandler, IDbContextFactory<DataContext> contextFact)
		{
			_serviceBusHandler = serviceBusHandler;
			_dbcontext = contextFact.CreateDbContext();
		}

		public async Task HandleImageExtractionHandler(Guid userId, Guid collectionId, BinaryData binaryData, IAnalyseImage imageAnlyser)
		{

			var reconisedCards = await imageAnlyser.AnalyseImage(binaryData);

			if (reconisedCards == null)
			{
				throw new Exception("No cards found in the image");
			}
			var scanItems = MapToScanItemObjects(reconisedCards, userId, collectionId);


			var scan = new ScanDatabaseModel();
			scan.Id = Guid.NewGuid();
			scan.CollectionId = collectionId;
			scan.ScanItems = new List<ScanItemsDatabaseModel>();
			foreach (var item in scanItems)
			{
				var scanItem = new ScanItemsDatabaseModel
				{
					Id = item.ScanItemId,
					UserId = userId,
					CardName = item.Name,
				};
				scan.ScanItems.Add(scanItem);
			}

			_dbcontext.Scans.Add(scan);

			await _dbcontext.SaveChangesAsync();



			var messages = SerializeConvertToJsonMessages(scanItems);
			await _serviceBusHandler.SendRangeMessageAsync(messages, MagicQueue.createQueue);

			Console.WriteLine($"Number of Cards: {reconisedCards.Lines.Count}\n" + string.Join("\n", reconisedCards.Lines.Select(line => line.Text)));
		}

		public async Task<ScanDatabaseModel> GetScanById(Guid id)
		{
			var scan = await _dbcontext.Scans
				.Include(x => x.ScanItems)
				.Include(x => x.Collection)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (scan == null)
			{
				throw new Exception("Scan not found");
			}

			return scan;

		}

		private List<CVCreateMessageModel> MapToScanItemObjects(ACVOCRResultModel reconisedCards, Guid userId, Guid collectionId)
		{
			var mappingList = new List<string>();
			var lines =  reconisedCards.Lines.Select(line => new CVCreateMessageModel
			{
				ScanItemId = Guid.NewGuid(),
				Name = line.Text,
				UserId = userId,
				CollectionId = collectionId,
				TimesProcessed = 0
			}).ToList();

			return lines;



		}

		private List<string> SerializeConvertToJsonMessages(List<CVCreateMessageModel> scanObjects)
		{
			var mappingList = new List<string>();
			foreach (var line in scanObjects)
			{
				var jsonMessage = JsonSerializer.Serialize(line);
				mappingList.Add(jsonMessage);
			}
			return mappingList;



		}

	}
}
