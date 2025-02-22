using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.IO;

namespace MagicScanner.BusinessLogic
{
    public static class Detection
    {

		public static async Task<string> CaptureAndAnalyzeCardAsync()
	{
		try
		{
			var photo = await Microsoft.Maui.Media.MediaPicker.CapturePhotoAsync();
			if (photo == null)
				return "No photo captured.";
 
			var stream = await photo.OpenReadAsync();
			return await AnalyzeImageAsync(stream);
		}
		catch (Exception ex)
		{
			return $"An error occurred: {ex.Message}";
		}
	}

		private static async Task<string> AnalyzeImageAsync(Stream imageStream)
		{
			string subscriptionKey = "9yU4zQ4RsYNYzmAdaABWJDLUfYyK5NSNmmZuNqxDv7snLfvgWyvUJQQJ99BBACi5YpzXJ3w3AAAFACOGY40l";
			string endpoint = "https://cs-vision.cognitiveservices.azure.com/";

			var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
			{
				Endpoint = endpoint
			};

			var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Description };
			var analysis = await client.AnalyzeImageInStreamAsync(imageStream, features);

			return analysis.Description.Captions.FirstOrDefault()?.Text ?? "No description found.";
		}
	}
}
