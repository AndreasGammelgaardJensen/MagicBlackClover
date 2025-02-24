using System.Windows.Input;
using MagicSC.Services;

namespace MagicSC.ViewModels
{
	public partial class ImageAnalysisViewModel : BaseViewModel
	{
		private readonly ImageAnalysisService _imageAnalysisService;

		public ImageAnalysisViewModel(ImageAnalysisService imageAnalysisService)
		{
			_imageAnalysisService = imageAnalysisService;
			CaptureAndAnalyzeCardCommand = new Command(async () => await CaptureAndAnalyzeCardAsync());
		}

		public ICommand CaptureAndAnalyzeCardCommand { get; }

		private async Task CaptureAndAnalyzeCardAsync()
		{
			try
			{
				var photo = await MediaPicker.CapturePhotoAsync();
				if (photo == null)
					return;

				using var stream = await photo.OpenReadAsync();
				using var memoryStream = new MemoryStream();
				await stream.CopyToAsync(memoryStream);
				var byteArray = memoryStream.ToArray();
				var binaryData = new BinaryData(byteArray);

				var response = await _imageAnalysisService.UploadImageAsync("analyse", Guid.NewGuid(), Guid.NewGuid(), stream, "GIVE ME A NEW NAME");
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					// Handle the result
					Result = result;
				}
				else
				{
					// Handle the error
					Result = "Error uploading image";
				}
			}
			catch (Exception ex)
			{
				Result = $"An error occurred: {ex.Message}";
			}
		}

		private string _result;
		public string Result
		{
			get => _result;
			set => SetProperty(ref _result, value);
		}
	}
}
