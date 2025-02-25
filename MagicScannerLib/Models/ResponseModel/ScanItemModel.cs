using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.ResponseModel
{
    public class ScanItemModel
    {
			public Guid Id { get; set; }
			public string CardName { get; set; }
			public Guid? CardId { get; set; }
	}
}
