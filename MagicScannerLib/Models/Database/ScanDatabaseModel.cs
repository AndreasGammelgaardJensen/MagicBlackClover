using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.Database
{
    public class ScanDatabaseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CollectionId { get; set; }
		public CollectionDatabaseModel Collection  { get; set; }
		public List<ScanItemsDatabaseModel> ScanItems { get; set; }

	}
}
