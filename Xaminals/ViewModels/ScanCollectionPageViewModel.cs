using MagicScannerLib.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xaminals.Services;

namespace Xaminals.ViewModels
{
	public class ScanCollectionPageViewModel : IQueryAttributable, INotifyPropertyChanged
	{
		private Guid _collectionId;
		private readonly ScanItemsService _scanService;
		private ObservableCollection<ScanItemModel> _scanItems;
		private ObservableCollection<ScanResponseModel> _batchItems;

		public event PropertyChangedEventHandler PropertyChanged;

		public ScanCollectionPageViewModel(ScanItemsService scanService)
		{
			_scanService = scanService;
			LoadDataCommand = new Command(async () => await LoadDataAsync());
		}

		public ICommand LoadDataCommand { get; }

		private async Task LoadDataAsync()
		{
			if (_collectionId == Guid.Empty)
			{
				// Collection is not set or Id is not valid
				return;
			}
			var items = await _scanService.GetScanByIdAsync(_collectionId);
			BatchItems = new ObservableCollection<ScanResponseModel>(items);
		}

		public ObservableCollection<ScanResponseModel> BatchItems
		{
			get => _batchItems;
			set
			{
				_batchItems = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<ScanItemModel> ScanItems
		{
			get => _scanItems;
			set
			{
				_scanItems = value;
				OnPropertyChanged();
			}
		}

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue("CollectionId", out var collectionId))
			{
				_collectionId = Guid.Parse(collectionId.ToString());
				LoadDataCommand.Execute(null);
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

