using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Azure.AI.Vision.ImageAnalysis;
using Azure;
using System.Text.Json;
using MagicScannerLib.Models;
using MagicScannerLib.Helper;
using MagicSC.Services;
using MagicSC.ViewModels;

namespace MagicSC
{
	public partial class MainPage : ContentPage
	{
		int count = 0;
		Guid collectionID = Guid.NewGuid();

		public MainPage(ImageAnalysisViewModel imageAnalysisService)
		{
			InitializeComponent();
			BindingContext = imageAnalysisService;
		}

		//private async void OnCaptureButtonClicked(object sender, EventArgs e)
		//{
		//	await CaptureAndAnalyzeCardAsync();
			
		//}

		//public async Task CaptureAndAnalyzeCardAsync()
		//{
		//	try
		//	{
		//		var photo = await MediaPicker.CapturePhotoAsync();
				

		//		BinaryData binaryData = null;
		//		using (var stream = await photo.OpenReadAsync())
		//		{
		//			using (var memoryStream = new MemoryStream())
		//			{
		//				await stream.CopyToAsync(memoryStream);
		//				var byteArray = memoryStream.ToArray();
		//				binaryData = new BinaryData(byteArray);
		//			}

		//			var response = await _imageAnalysisService.UploadImageAsync("/analyse", collectionID, stream, "GIVE ME A NEW NAME");
		//			if (response.IsSuccessStatusCode)
		//			{
		//				var result = await response.Content.ReadAsStringAsync();
		//				// Handle the result
		//				Console.WriteLine(result);
		//			}
		//			else
		//			{
		//				// Handle the error
		//				Console.WriteLine("Error uploading image");
		//			}
		//		}


		//		//var stream5 = await photo.OpenReadAsync();
		//		//return await AnalyzeImageAsync(stream5);
		//		//return AnalyzeImage(binaryData);
		//	}
		//	catch (Exception ex)
		//	{
				
		//	}
		//}

		private async Task<string> AnalyzeImageAsync(Stream imageStream)
		{
			string subscriptionKey = "9yU4zQ4RsYNYzmAdaABWJDLUfYyK5NSNmmZuNqxDv7snLfvgWyvUJQQJ99BBACi5YpzXJ3w3AAAFACOGY40l";
			string endpoint = "https://cs-vision.cognitiveservices.azure.com/";

			var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
			{
				Endpoint = endpoint
			};

			// Create a copy of the stream for OCR
			var memoryStream = new MemoryStream();
			await imageStream.CopyToAsync(memoryStream);
			memoryStream.Position = 0;
			imageStream.Position = 0;

			// Detect objects in the image
			var detectResult = await client.DetectObjectsInStreamAsync(imageStream);

			// Perform OCR on the copied stream
			var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, memoryStream);


			foreach (var region in ocrResult.Regions)
			{
				region.Lines = region.Lines.OrderBy(line => ParseBoundingBox(line.BoundingBox)).ToList();
			}



			// Extract information for each detected card
			var cardInfoList = new List<string>();
			foreach (var detectedObject in detectResult.Objects)
			{

				var cardTitle = ExtractTitle(ocrResult, detectedObject.Rectangle);
				var cardType = ExtractCardType(ocrResult, detectedObject.Rectangle);
				cardInfoList.Add($"Title: {cardTitle}\nCard Type: {cardType}");

			}

			return $"Number of Cards: {cardInfoList.Count}\n\n" + string.Join("\n\n", cardInfoList);
		}

		private int ParseBoundingBox(string boundingBox)
		{
			// Parse the bounding box string into its integer components
			var parts = boundingBox.Split(',').Select(int.Parse).ToArray();
			return parts[0]; // Return the x-coordinate of the left edge for sorting
		}

		private string ExtractTitle(OcrResult ocrResult, BoundingRect boundingRect)
		{
			// Implement logic to extract the title from the OCR result within the bounding rectangle
			var lines = ocrResult.Regions.SelectMany(region => region.Lines)
										 .Where(line => IsWithinBoundingRect(line, boundingRect))
										 .ToList();

			return lines.FirstOrDefault()?.Words
						.Select(word => word.Text)
						.Aggregate((current, next) => current + " " + next) ?? "Title not found";
		}

		private string ExtractCardType(OcrResult ocrResult, BoundingRect boundingRect)
		{
			// Implement logic to extract the card type from the OCR result within the bounding rectangle
			var lines = ocrResult.Regions.SelectMany(region => region.Lines)
										 .Where(line => IsWithinBoundingRect(line, boundingRect))
										 .Skip(1) // Assuming the card type is in the second line
										 .ToList();

			return lines.FirstOrDefault()?.Words
						.Select(word => word.Text)
						.Aggregate((current, next) => current + " " + next) ?? "Card type not found";
		}

		private bool IsWithinBoundingRect(OcrLine line, BoundingRect boundingRect)
		{
			// Implement logic to check if the line is within the bounding rectangle
			var lineRect = new BoundingRect
			{
				X = line.Words.Min(word => word.BoundingBox[0]),
				Y = line.Words.Min(word => word.BoundingBox[1]),
				W = line.Words.Max(word => word.BoundingBox[2]) - line.Words.Min(word => word.BoundingBox[0]),
				H = line.Words.Max(word => word.BoundingBox[3]) - line.Words.Min(word => word.BoundingBox[1])
			};

			return lineRect.X >= boundingRect.X &&
				   lineRect.Y >= boundingRect.Y &&
				   lineRect.X + lineRect.W <= boundingRect.X + boundingRect.W &&
				   lineRect.Y + lineRect.H <= boundingRect.Y + boundingRect.H;
		}

		static string AnalyzeImage(BinaryData data)
		{
			string subscriptionKey = "9yU4zQ4RsYNYzmAdaABWJDLUfYyK5NSNmmZuNqxDv7snLfvgWyvUJQQJ99BBACi5YpzXJ3w3AAAFACOGY40l";
			string endpoint = "https://cs-vision.cognitiveservices.azure.com/";

			ImageAnalysisClient client = new ImageAnalysisClient(
				new Uri(endpoint),
				new AzureKeyCredential(subscriptionKey));

			ImageAnalysisResult result = client.Analyze(
				data,
				VisualFeatures.Caption | VisualFeatures.Read,
				new ImageAnalysisOptions { GenderNeutralCaption = true });


			var blockJson = JsonSerializer.Serialize(result.Read.Blocks);


			var res = JsonSerializer.Deserialize<List<ACVOCRResultModel>>(blockJson, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			var list = res.SelectMany(block => block.Lines).ToList();

			var centroids = list.Select(line => line.GetCentroid()).ToList();

			// Remove centroids that are too close to each other
			var filteredCentroids = GeometryHelper.RemoveCloseCentroids(centroids, 100);

			//Find the lineopject for each remaning centriod and print the text
			foreach (var centroid in filteredCentroids)
			{
				var line = list.FirstOrDefault(l => l.GetCentroid().DistanceTo(centroid) < 5);
				Console.WriteLine(line.Text);
			}

			//filter the results.lines to only contain the ones that are in the filteredCentroids
			list = list.Where(line => filteredCentroids.Any(fc => line.GetCentroid().DistanceTo(fc) == 0.0)).ToList();

			//Join and return all test
			return $"Number of Cards: {list.Count}\n" +  string.Join("\n", list.Select(line => line.Text));



			//Console.WriteLine("Image analysis results:");
			//Console.WriteLine(" Caption:");
			//Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");

			//Console.WriteLine(" Read:");
			//var cardInfoList = new List<string>();
			//foreach (DetectedTextBlock block in result.Read.Blocks)
			//	foreach (DetectedTextLine line in block.Lines)
			//	{
			//		cardInfoList.Add(line.Text);
			//		foreach (DetectedTextWord word in line.Words)
			//		{
			//			Console.WriteLine($"     Word: '{word.Text}', Confidence {word.Confidence.ToString("#.####")}, Bounding Polygon: [{string.Join(" ", word.BoundingPolygon)}]");
			//		}
			//	}

			//return $"Number of Cards: {cardInfoList.Count}\n\n" + string.Join("\n\n", cardInfoList);
		}
	}

	
		
	}