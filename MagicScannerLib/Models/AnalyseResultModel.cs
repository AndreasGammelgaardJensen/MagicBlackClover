﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Models
{
    public class AnalyseResultModel
    {
        public int userId { get; set; }
        public Guid orderId { get; set; }
        public Guid collectionId { get; set; }
    }
}
