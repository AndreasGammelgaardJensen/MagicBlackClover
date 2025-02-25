using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.ResponseModel
{
	public class Collection
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<Card> Cards { get; set; }
	}
}
