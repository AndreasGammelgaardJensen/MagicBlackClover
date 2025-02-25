using MagicScannerLib.Models.ResponseModel;
using System.Collections.ObjectModel;
using Xaminals.Extensions;
using Xaminals.Services;
using Xaminals.ViewModels;

namespace Xaminals.Views;

public partial class CollectionsPage : ContentPage
{
	private readonly CollectionPageViewModel _viewModel;

	public CollectionsPage()
	{
		InitializeComponent();

		var httpClient = new HttpClient();
		var collectionService = new CollectionService(httpClient);
		_viewModel = new CollectionPageViewModel(collectionService);

		BindingContext = _viewModel;
		_viewModel.LoadDataCommand.Execute(null);

		static async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (((CollectionView)sender).ClearSelection())
				return;

			Collection collection = e.CurrentSelection.FirstOrDefault() as Collection;
			var navigationParameters = new Dictionary<string, object>
			{
				{ "Collection", collection }
			};
			await Shell.Current.GoToAsync($"collectiondetails", navigationParameters);
		}
	}
}