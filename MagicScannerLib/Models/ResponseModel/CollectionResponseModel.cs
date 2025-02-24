using MagicScannerLib.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.ResponseModel
{
    public class CollectionResponseModel
    {
        public Guid Id { get; set; }
		public string CollectionName { get; set; }
		public List<CardDatabaseModel> Cards { get; set; }
	}
}
