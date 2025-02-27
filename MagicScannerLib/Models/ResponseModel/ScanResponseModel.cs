﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models.ResponseModel
{
	public class ScanResponseModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string BatchId { get; set; }
		public Guid CollectionId { get; set; }
		public List<ScanItemModel> ScanItems { get; set; }
	}
}
