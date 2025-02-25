using MagicScannerLib.Models.ResponseModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xaminals.Services;

namespace Xaminals.ViewModels
{
	public class CollectionDetailViewModel : IQueryAttributable, INotifyPropertyChanged
	{
		private Collection _collection;
		
		private ObservableCollection<Card> _collectionCards;
		private Card _selectedCard;

		public event Action AddButtonsRequested;




		public CollectionDetailViewModel()
		{
			FloatingButtonCommand = new Command(OnFloatingButtonClicked);
		}

		public Collection Collection
		{
			get => _collection;
			private set
			{
				_collection = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<Card> CollectionCards
		{
			get => _collectionCards;
			private set
			{
				_collectionCards = value;
				OnPropertyChanged();
			}
		}

		public Card SelectedCard
		{
			get => _selectedCard;
			set
			{
				if (_selectedCard != value)
				{
					_selectedCard = value;
					OnPropertyChanged();
					OnCardSelected();
				}
			}
		}

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue("Collection", out var collection))
			{
				Collection = collection as Collection;
				CollectionCards = new ObservableCollection<Card>(Collection?.Cards ?? new List<Card>());
			}
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand FloatingButtonCommand { get; }

		private void OnFloatingButtonClicked()
		{
			// Handle the button click event here
			AddButtonsRequested?.Invoke();
		}
    

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async void OnCardSelected()
		{
			if (SelectedCard == null)
				return;

			var navigationParameters = new Dictionary<string, object>
		{
			{ "Card", SelectedCard }
		};
			await Shell.Current.GoToAsync($"cardetails", navigationParameters);

			// Clear selection
			SelectedCard = null;
		}

		#endregion
	}
}
