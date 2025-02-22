using System.Text.RegularExpressions;
using static MagicScannerLib.Models.ACVOCRResultModel;

namespace MagicScannerLib.Helper
{
	public static class GeometryHelper
	{
		public static class PolygonCentroid
		{
			public static Point CalculateCentroid(List<Point> vertices)
			{
				if (vertices == null || vertices.Count < 3)
					throw new ArgumentException("A polygon must have at least three vertices.");

				double signedArea = 0;
				double centroidX = 0;
				double centroidY = 0;
				int count = vertices.Count;

				for (int i = 0; i < count; i++)
				{
					int next = (i + 1) % count;
					double x0 = vertices[i].X, y0 = vertices[i].Y;
					double x1 = vertices[next].X, y1 = vertices[next].Y;

					double crossProduct = (x0 * y1 - x1 * y0);
					signedArea += crossProduct;
					centroidX += (x0 + x1) * crossProduct;
					centroidY += (y0 + y1) * crossProduct;
				}

				signedArea *= 0.5;
				if (signedArea == 0) throw new InvalidOperationException("The polygon has zero area.");

				centroidX /= (6 * signedArea);
				centroidY /= (6 * signedArea);

				return new Point { X = centroidX, Y = centroidY };
			}
		}



		public static List<Point> RemoveCloseCentroids(List<Point> centroids, double threshold = 210)
		{
			if (centroids == null || centroids.Count < 2)
				return centroids;

			List<Point> filteredCentroids = new List<Point>();

			foreach (var centroid in centroids)
			{
				var copy = centroids.Where(x => !x.Equals(centroid));

				if (copy.All(existing => existing.DistanceTo(centroid) >= threshold))
				{
					filteredCentroids.Add(centroid);
				}
			}

			return filteredCentroids;
		}


		public static List<Line> RemoveLineIfTextContainsNumber(List<Line> line)
		{
			List<Point> filteredCentroids = new List<Point>();

			var filtered = line.Where(x => !ContainsNumberOrSpecialCharacter(x.Text)).ToList();
			return filtered;
		}

		public static bool ContainsNumberOrSpecialCharacter(string input)
		{
			if (string.IsNullOrEmpty(input))
				return false;

			var regex = new Regex(@"[\d]");
			return regex.IsMatch(input);
		}


	}
}
