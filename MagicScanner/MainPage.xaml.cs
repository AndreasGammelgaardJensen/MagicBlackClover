using MagicScanner.BusinessLogic;


namespace MagicScanner
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private async void OnCaptureButtonClicked(object sender, EventArgs e)
		{
			var result = await Detection.CaptureAndAnalyzeCardAsync();
			ResultLabel.Text = result;
		}
	}

}
