using Azure;
using Azure.AI.Vision.ImageAnalysis;
using MagicScannerLib.Helper;
using MagicScannerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Schema;
using static MagicScannerLib.Models.ACVOCRResultModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicScannerLib.ComputerVision
{
	public class ImageTextRecognitionAnalyser : IAnalyseImage
	{
		public async Task<ACVOCRResultModel> AnalyseImage(BinaryData binaryData)
		{
			string subscriptionKey = "";
			string endpoint = "";

			ImageAnalysisClient client = new ImageAnalysisClient(
				new Uri(endpoint),
				new AzureKeyCredential(subscriptionKey));

			ImageAnalysisResult result = await client.AnalyzeAsync(
				binaryData,
				VisualFeatures.Caption | VisualFeatures.Read,
				new ImageAnalysisOptions { GenderNeutralCaption = true });


			var blockJson = JsonSerializer.Serialize(result.Read.Blocks);


			var res = JsonSerializer.Deserialize<List<ACVOCRResultModel>>(blockJson, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			var list = res.SelectMany(block => block.Lines).ToList();

			list = GeometryHelper.RemoveLineIfTextContainsNumber(list);

			var centroids = list.Select(line => line.GetCentroid()).ToList();
			var extremes = FindExtremes(centroids);
			var average = CalculateAverageCoordinates(centroids);
			var avgXDistance = CalculateAverageXDistance(centroids, extremes.smallestX.X);
			var avgY = CalculateAverageYDistance(centroids, extremes.smallestY.Y);
			var avg = (extremes.smallestX.X + extremes.smallestY.Y) / 2;
			// Remove centroids that are too close to each other
			//How to select the value that is smalles? 

			var filteredCentroids = GeometryHelper.RemoveCloseCentroids(centroids, 205);

			//filter the results.lines to only contain the ones that are in the filteredCentroids
			list = list.Where(line => filteredCentroids.Any(fc => line.GetCentroid().DistanceTo(fc) == 0.0)).ToList();

			var returnResult = res.FirstOrDefault();
			returnResult.Lines = list;
			return returnResult;
		}


		private Point CalculateAverageCoordinates(List<Point> points)
		{
			if (points == null || points.Count == 0)
				throw new ArgumentException("The points list cannot be null or empty.");

			double averageX = points.Average(p => p.X);
			double averageY = points.Average(p => p.Y);

			return new Point
			{
				X = averageX,
				Y = averageY
			};
		}


		private (Point largestX, Point smallestX, Point largestY, Point smallestY) FindExtremes(List<Point> points)
		{
			if (points == null || points.Count == 0)
				throw new ArgumentException("The points list cannot be null or empty.");

			var largestX = points.OrderByDescending(p => p.X).First();
			var smallestX = points.OrderBy(p => p.X).First();
			var largestY = points.OrderByDescending(p => p.Y).First();
			var smallestY = points.OrderBy(p => p.Y).First();

			return (largestX, smallestX, largestY, smallestY);
		}

		private double CalculateAverageXDistance(List<Point> points, double smallestX)
		{
			if (points == null || points.Count < 2)
				throw new ArgumentException("The points list must contain at least two points.");

			double totalDistance = 0;
			int count = 0;

			for (int i = 0; i < points.Count; i++)
			{
				
				totalDistance += Math.Abs(totalDistance + (points[i].X - smallestX));
				count++;
				
			}

			return totalDistance / count;
		}

		private double CalculateAverageYDistance(List<Point> points, double smallestY)
		{
			if (points == null || points.Count < 2)
				throw new ArgumentException("The points list must contain at least two points.");

			double totalDistance = 0;
			int count = 0;

			for (int i = 0; i < points.Count; i++)
			{
				totalDistance += Math.Abs(totalDistance + (points[i].Y - smallestY));
				count++;
			}

			return totalDistance / count;
		}
	}
}
