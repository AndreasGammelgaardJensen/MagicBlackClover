using Xaminals.Services;
using Xaminals.ViewModels;

namespace Xaminals.Views;

public partial class ScanCollectionPage : ContentPage
{
	private readonly ScanCollectionPageViewModel _viewModel;

	public ScanCollectionPage()
	{
		var httpClient = new HttpClient();
		var collectionService = new ScanItemsService();
		InitializeComponent();
		_viewModel = new ScanCollectionPageViewModel(new ScanItemsService());
		BindingContext = _viewModel;

	}
}