using DataAccess;
using MagicScannerLib.Messaging.AzureServiceBus;
using MagicScannerLib.Models.Database;
using MagicScannerLib.Models.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace MagicScanner.API.Handlers
{
	public class CollectionHandler
	{
		private readonly DataContext _dbcontext;


		public CollectionHandler(IDbContextFactory<DataContext> contextFact)
		{
			_dbcontext = contextFact.CreateDbContext();
		}

		public async Task<Guid> CreateCollection(Guid userId, string collectionName)
		{
			var collection = new CollectionDatabaseModel
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				CollectionName = collectionName
			};
			_dbcontext.Collections.Add(collection);
			await _dbcontext.SaveChangesAsync();

			return collection.Id;
		}

		public async Task<List<CollectionDatabaseModel>> GetCollections(Guid userId)
		{
			var collection = await _dbcontext.Collections.Where(c => c.UserId == userId).ToListAsync();

			return collection;
		}

		public async Task<CollectionResponseModel> GetCollection(Guid collectionId)
		{
			var collection = await _dbcontext.Collections.Where(c => c.Id == collectionId).Include(x=>x.CollectionCards).ThenInclude(cc=>cc.Card).Select(x=>new { Name = x.CollectionName, Cards = x.CollectionCards.Select(c=>c.Card) }).FirstOrDefaultAsync();

			var collectionName = collection.Name;
			var cards = collection.Cards.ToList();

			return new CollectionResponseModel { Id=collectionId, Cards=cards, CollectionName= collectionName };
		}

	}
}
