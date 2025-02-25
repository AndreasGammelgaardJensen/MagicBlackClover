using Xaminals.ViewModels;
using System.Collections.Generic;
using Xaminals.Extensions;
using Xaminals.Services;
using Microsoft.Maui.Layouts;
using System.Threading.Tasks;
using MagicScannerLib.Models;

namespace Xaminals.Views;

public partial class CollectionDetailPage : ContentPage
{
	private readonly CollectionDetailViewModel _viewModel;
	private Button _button1;
	private Button _button2;

	public CollectionDetailPage()
	{
		InitializeComponent();
		_viewModel = new CollectionDetailViewModel();
		BindingContext = _viewModel;
		_viewModel.AddButtonsRequested += AddButtonsRequested;
	}

	static async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (((CollectionView)sender).ClearSelection())
			return;

		Card collection = e.CurrentSelection.FirstOrDefault() as Card;
		var navigationParameters = new Dictionary<string, object>
			{
				{ "Collection", collection }
			};
		await Shell.Current.GoToAsync($"collectiondetails", navigationParameters);
	}

	private void AddButtonsRequested()
	{
		// Handle the button click event her

		// Create Button1 and Button2
		_button1 = new Button
		{
			Text = "+",
			BackgroundColor = Colors.LightGray,
			TextColor = Colors.Black,
			CornerRadius = 30,
			WidthRequest = 30,
			HeightRequest = 30
		};

		_button2 = new Button
		{
			Text = "s",
			BackgroundColor = Colors.LightGray,
			TextColor = Colors.Black,
			CornerRadius = 30,
			WidthRequest = 30,
			HeightRequest = 30
		};

		// Add buttons to the AbsoluteLayout
		AbsoluteLayout.SetLayoutBounds(_button1, new Rect(0.85, 0.89, 10, 10));
		AbsoluteLayout.SetLayoutFlags(_button1, AbsoluteLayoutFlags.PositionProportional);
		AbsoluteLayout.SetLayoutBounds(_button2, new Rect(0.79, 0.95, 10, 10));
		AbsoluteLayout.SetLayoutFlags(_button2, AbsoluteLayoutFlags.PositionProportional);

		absoluteLayout.Children.Add(_button1);
		absoluteLayout.Children.Add(_button2);
		

		_button2.Clicked += async (object sender, EventArgs e) =>
		{
			
			var navigationParameters = new Dictionary<string, object>
				{
					{ "CollectionId", _viewModel.Collection.Id } // Replace with actual key-value pairs
				};

			// Correct way to pass parameters using query parameters

			await Shell.Current.GoToAsync($"scancollections", navigationParameters);
		};
	}

	private void OnAbsoluteLayoutTapped(object sender, EventArgs e)
	{
		// Remove Button1 and Button2 if they exist
		if (_button1 != null && absoluteLayout.Children.Contains(_button1))
		{
			absoluteLayout.Children.Remove(_button1);
			_button1 = null;
		}

		if (_button2 != null && absoluteLayout.Children.Contains(_button2))
		{
			absoluteLayout.Children.Remove(_button2);
			_button2 = null;
		}
	}
}
