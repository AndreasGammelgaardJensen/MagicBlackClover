
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using DataAccess;
using MagicScannerLib.Messaging;
using MagicScannerLib.Messaging.AzureServiceBus;
using MagicScannerLib.Models;
using MagicScannerLib.Models.Database;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CreateMGTFunc
{
	public class fc_downloadCardFunc
	{
		private readonly ILogger<fc_downloadCardFunc> _logger;
		private readonly IDbContextFactory<DataContext> _contextFactory;
		private readonly ServiceBusHandler _serviceBusHandler;
		private readonly IHttpClientFactory _httpClientFactory;

		public fc_downloadCardFunc(ILogger<fc_downloadCardFunc> logger, IDbContextFactory<DataContext> contextFactory, ServiceBusHandler serviceBusHandler, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_contextFactory = contextFactory;
			_serviceBusHandler = serviceBusHandler;
			_httpClientFactory = httpClientFactory;
		}

		[Function(nameof(fc_downloadCardFunc))]
		public async Task Run(
			[ServiceBusTrigger(MagicQueue.downloadQueue, Connection = "ServiceBusSettings:ConnectionString")]
			ServiceBusReceivedMessage message,
			ServiceBusMessageActions messageActions)
		{
			var messageModel = JsonSerializer.Deserialize<CVCreateMessageModel>(message.Body);

			_logger.LogInformation("Message ID: {id}", message.MessageId);
			_logger.LogInformation("Message Body: {body}", message.Body);
			_logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

			// Create an HttpClient instance using the factory
			var client = _httpClientFactory.CreateClient("MTGAPI");

			var uriBuilder = new UriBuilder(client.BaseAddress + "cards");
			var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
			query["name"] = messageModel.Name;
			uriBuilder.Query = query.ToString();
			var url = uriBuilder.ToString();

			// Call the MTG API
			var response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var mtgApiResponse = JsonSerializer.Deserialize<MTGApiResponse>(content);

				using (var context = _contextFactory.CreateDbContext())
				{
					foreach (var card in mtgApiResponse.Cards)
					{
						var cardEntity = new CardDatabaseModel
						{
							Id = Guid.NewGuid(),
							Name = card.Name,
							ManaCost = card.ManaCost,
							Cmc = card.Cmc,
							Type = (int)CardTypePapper.MapStringToCardType(card.Type),
							TypeDescription = card.Type,
							Rarity = card.Rarity,
							Set = card.Set,
							SetName = card.SetName,
							Text = card.Text,
							Artist = card.Artist,
							Number = card.Number,
							Power = card.Power,
							Toughness = card.Toughness,
							Layout = card.Layout,
							MultiverseId = card.MultiverseId,
							ImageUrl = card.ImageUrl,
							ForeignNames = card.ForeignNames?.ConvertAll(fn => new ForeignNameDatabaseModel
							{
								Id = Guid.NewGuid(),
								Name = fn.Name,
								Language = fn.Language,
								MultiverseId = fn.MultiverseId
							}),
							Legalities = card.Legalities?.ConvertAll(l => new LegalityDatabaseModel
							{
								Id = Guid.NewGuid(),
								Format = l.Format,
								LegalityStatus = l.LegalityStatus
							}),
							Printings = card.Printings?.ConvertAll(p => new PrintingDatabaseModel
							{
								Id = Guid.NewGuid(),
								Set = p
							})
						};

						context.Cards.Add(cardEntity);
					}

					await context.SaveChangesAsync();
				}

				_logger.LogInformation("MTG API Response: {content}", content);
				await _serviceBusHandler.SendMessageAsync(JsonSerializer.Serialize(messageModel), MagicQueue.createQueue);
				await messageActions.CompleteMessageAsync(message);
			}
			else
			{
				_logger.LogError("Failed to fetch data from MTG API. Status Code: {statusCode}", response.StatusCode);
				throw new Exception($"Failed to fetch data from MTG API. Status Code: {response.StatusCode}, message: {JsonSerializer.Serialize(messageModel)}");
			}


			// Complete the message
		}
	}
}
