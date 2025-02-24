using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.Database
{
	public class CollectionCardsDatabaseModel
	{
		public Guid CollectionId { get; set; }
		public CollectionDatabaseModel Collection { get; set; }
		public Guid CardId { get; set; }
		public CardDatabaseModel Card { get; set; }
	}
}
