using System.Collections.Generic;

namespace Kengic.Was.Operator.Sorter.Arithmetics
{
    public interface IBarcodeFormat
    {
        bool FormatBarcode(string scannerBarcode, out BarcodeInfomation barcodeInfomation);
    }

    public class BarcodeInfomation
    {
        public string Barcode { get; set; }
        public string PackageBarCode { get; set; }

        public string OrderBarCode { get; set; }

        public List<string> BarcodeList { get; set; }

        public bool IfMultiBracode { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Volume { get; set; }

        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }
    }
}