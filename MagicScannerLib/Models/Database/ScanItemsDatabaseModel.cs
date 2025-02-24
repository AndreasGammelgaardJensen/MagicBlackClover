using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.Database
{
    public class ScanItemsDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CardName { get; set; }
        public Guid? CardId { get; set; }
		public CardDatabaseModel? Card { get; set; }
	}
}
