using MagicScannerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.ComputerVision
{
    public interface IAnalyseImage
    {
		Task<ACVOCRResultModel> AnalyseImage(BinaryData binaryData);
	}
}
