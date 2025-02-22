using System;
using OpenCvSharp;

class Program
{
	static void Main()
	{
		string inputImage = @"20250214_232935.jpg";
		string outputDir = "";

		// Load the image
		Mat src = Cv2.ImRead(inputImage);
		Mat gray = new Mat();
		Mat blurred = new Mat();
		Mat edges = new Mat();

		// Convert to grayscale
		Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

		// Apply Gaussian Blur
		Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

		// Canny Edge Detection
		Cv2.Canny(blurred, edges, 50, 150);

		// Find contours
		Point[][] contours;
		HierarchyIndex[] hierarchy;
		Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

		int cardCount = 0;
		foreach (var contour in contours)
		{
			// Approximate contour to polygon
			Point[] approx = Cv2.ApproxPolyDP(contour, 0.02 * Cv2.ArcLength(contour, true), true);

			// Check if the approximated contour has 4 points (rectangle)
			if (approx.Length == 4)
			{
				// Check area to filter out small contours
				double area = Cv2.ContourArea(approx);
				if (area > 10000) // Adjust this threshold as needed
				{
					// Get bounding rectangle
					Rect boundingRect = Cv2.BoundingRect(approx);

					// Crop the card from the original image
					Mat card = new Mat(src, boundingRect);

					// Save each card as a separate image file
					string outputPath = $"{outputDir}card_{cardCount}.jpg";
					Cv2.ImWrite(outputPath, card);
					Console.WriteLine($"Saved: {outputPath}");

					cardCount++;
				}
			}
		}

		Console.WriteLine("Card detection and cropping completed!");
	}
}
