using MagicScannerLib.ComputerVision;
using System.Collections.Generic;

namespace MagicScanner.API.Handlers
{
	public static class HandleImageExtraction
	{
	
		public static async Task HandleImageExtractionHandler(Guid userId, BinaryData binaryData, IAnalyseImage imageAnlyser)
		{

			var reconisedCards = await imageAnlyser.AnalyseImage(binaryData);

			if (reconisedCards == null)
			{
				throw new Exception("No cards found in the image");
			}


			//Store all names to database and add processeng

			//Add to queue

			//Create and ordernumber and navigate to that page

			//Join and return all test
			Console.WriteLine($"Number of Cards: {reconisedCards.Lines.Count}\n" + string.Join("\n", reconisedCards.Lines.Select(line => line.Text)));



		}
	}
}
