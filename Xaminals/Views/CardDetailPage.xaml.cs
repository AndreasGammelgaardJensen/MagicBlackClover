using Xaminals.ViewModels;

namespace Xaminals.Views;

public partial class CardDetailPage : ContentPage
{
	private readonly CardDetailViewModel _viewModel;

	public CardDetailPage()
	{
		InitializeComponent();
		_viewModel = new CardDetailViewModel();
		BindingContext = _viewModel;
	}
}