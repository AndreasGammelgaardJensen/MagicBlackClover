
using System.Text.Json.Serialization;
using static MagicScannerLib.Helper.GeometryHelper;

namespace MagicScannerLib.Models
{
	public class ACVOCRResultModel
	{
		[JsonPropertyName("Lines")]
		public List<Line> Lines { get; set; }

		public class Line
		{
			[JsonPropertyName("text")]
			public string Text { get; set; }

			[JsonPropertyName("boundingPolygon")]
			public List<Point> BoundingPolygon { get; set; }

			[JsonPropertyName("words")]
			public List<Word> Words { get; set; }

			public Point GetCentroid()
			{
				return PolygonCentroid.CalculateCentroid(BoundingPolygon);
			}
		}

		public class Point
		{
			[JsonPropertyName("x")]
			public double X { get; set; }

			[JsonPropertyName("y")]
			public double Y { get; set; }

			public double DistanceTo(Point other)
			{
				return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
			}
		}

		public class Word
		{
			[JsonPropertyName("text")]
			public string Text { get; set; }

			[JsonPropertyName("boundingPolygon")]
			public List<Point> BoundingPolygon { get; set; }

			[JsonPropertyName("confidence")]
			public double Confidence { get; set; }
		}
	}

	public class LineComparer : IComparer<ACVOCRResultModel.Line>
	{
		private const int Tolerance = 5;

		public int Compare(ACVOCRResultModel.Line line1, ACVOCRResultModel.Line line2)
		{
			var centroid1 = line1.GetCentroid();
			var centroid2 = line2.GetCentroid();

			if (Math.Abs(centroid1.Y - centroid2.Y) <= Tolerance)
			{
				return centroid1.X.CompareTo(centroid2.X);
			}

			return centroid1.Y.CompareTo(centroid2.Y);
		}
	}
}
