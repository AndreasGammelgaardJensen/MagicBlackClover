using MagicScannerLib.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xaminals.Services
{
	public class ScanItemsService
	{
		private readonly List<ScanResponseModel> _scans;

		public ScanItemsService()
		{
			// Initialize with some sample data
			_scans = new List<ScanResponseModel>
			{
				new ScanResponseModel
				{
					Id = Guid.NewGuid(),
					UserId = Guid.NewGuid(),
					BatchId = "Batch 1",
					CollectionId = Guid.Parse("62B9B6F4-CCAA-4DE0-AE7B-E1AF4E6ADD63"),
					ScanItems = new List<ScanItemModel>
					{
						new ScanItemModel { Id = Guid.NewGuid(), CardName = "Card 1", CardId = Guid.NewGuid() },
						new ScanItemModel { Id = Guid.NewGuid(), CardName = "Card 2", CardId = null }
					}
				},

				new ScanResponseModel
				{
					Id = Guid.NewGuid(),
					UserId = Guid.NewGuid(),
					BatchId = "Batch 2",
					CollectionId = Guid.Parse("62B9B6F4-CCAA-4DE0-AE7B-E1AF4E6ADD63"),
					ScanItems = new List<ScanItemModel>
					{
						new ScanItemModel { Id = Guid.NewGuid(), CardName = "Card 2 - 1", CardId = Guid.NewGuid() },
						new ScanItemModel { Id = Guid.NewGuid(), CardName = "Card 2 - 2", CardId = null }
					}
				},
				new ScanResponseModel
				{
					Id = Guid.NewGuid(),
					UserId = Guid.NewGuid(),
					BatchId = "Batch 1",
					CollectionId = Guid.Parse("34C1947F-B4C9-4855-AFE3-597A600AC8A4"),
					ScanItems = new List<ScanItemModel>
					{
						new ScanItemModel { Id = Guid.NewGuid(), CardName = "Card 3", CardId = Guid.NewGuid() },
						new ScanItemModel { Id = Guid.NewGuid(), CardName = "Card 4", CardId = null }
					}
				}
			};
		}

		public Task<List<ScanResponseModel>> GetScansAsync()
		{
			return Task.FromResult(_scans);
		}

		public Task<List<ScanResponseModel>> GetScanByIdAsync(Guid collectionId)
		{
			var scans = _scans.Where(s => s.CollectionId == collectionId).ToList();
			return Task.FromResult(scans);
		}

		public Task AddScanAsync(ScanResponseModel scan)
		{
			_scans.Add(scan);
			return Task.CompletedTask;
		}

		public Task DeleteScanAsync(Guid id)
		{
			var scan = _scans.FirstOrDefault(s => s.Id == id);
			if (scan != null)
			{
				_scans.Remove(scan);
			}
			return Task.CompletedTask;
		}
	}
}
