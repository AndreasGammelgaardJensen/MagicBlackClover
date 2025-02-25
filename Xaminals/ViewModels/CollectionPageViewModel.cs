using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xaminals.Services;
using Microsoft.Maui.Controls;
using MagicScannerLib.Models.ResponseModel;

namespace Xaminals.ViewModels
{
	public class CollectionPageViewModel : BindableObject
	{
		private readonly CollectionService _collectionService;
		private ObservableCollection<Collection> _collectionItems;
		private Collection _selectedCollection;

		public CollectionPageViewModel(CollectionService collectionService)
		{
			_collectionService = collectionService;
			LoadDataCommand = new Command(async () => await LoadDataAsync());
		}

		public ObservableCollection<Collection> CollectionItems
		{
			get => _collectionItems;
			set
			{
				_collectionItems = value;
				OnPropertyChanged();
			}
		}

		public Collection SelectedCollection
		{
			get => _selectedCollection;
			set
			{
				if (_selectedCollection != value)
				{
					_selectedCollection = value;
					OnPropertyChanged();
					OnCollectionSelected();
				}
			}
		}

		public ICommand LoadDataCommand { get; }

		private async Task LoadDataAsync()
		{
			var items = await _collectionService.GetCollectionItemsAsync();
			CollectionItems = new ObservableCollection<Collection>(items);
		}

		private async void OnCollectionSelected()
		{
			if (SelectedCollection == null)
				return;

			var navigationParameters = new Dictionary<string, object>
		{
			{ "Collection", SelectedCollection }
		};
			await Shell.Current.GoToAsync($"collectiondetails", navigationParameters);

			// Clear selection
			SelectedCollection = null;
		}
	}
}


