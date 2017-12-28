using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.Operator.Sorter.Parameters;
using System.Configuration;

namespace Kengic.Was.Operator.Sorter.Arithmetics
{
    public class CommonBarcodeFormat : IBarcodeFormat
    {
        public bool FormatBarcode(string scannerBarcode, out BarcodeInfomation barcodeInfomation)
        {
            //<STX>30|61|0123|L0320W0250H0200|X0250Y0200|03|12;117050538359;12;186558502786;12;999896987458|<EXT><CR>
            //< STX > 30 | 61 | 0123 | L0320W0250H0200 | X0250Y0200 | 00 |< EXT >< CR >
            barcodeInfomation = new BarcodeInfomation
            {
                Barcode = "",
                BarcodeList = new List<string>(),
                IfMultiBracode = false,
                PackageBarCode = ""
            };

            var barCodeArray = scannerBarcode.Split('|');

            if (barCodeArray.Length < 5) return false;

            var articalSize = barCodeArray[3];
            if (articalSize.Length == 15)
            {
                barcodeInfomation.Length = Convert.ToInt16(articalSize.Substring(1, 4));
                barcodeInfomation.Width = Convert.ToInt16(articalSize.Substring(6, 4));
                barcodeInfomation.Height = Convert.ToInt16(articalSize.Substring(11, 4));
                barcodeInfomation.Volume = articalSize.Substring(1, 4) + articalSize.Substring(6, 4) +
                                           articalSize.Substring(11, 4);
            }

            var articalPostion = barCodeArray[4];
            if (articalPostion.Length == 10)
            {
                barcodeInfomation.CoordinateX = Convert.ToInt16(articalPostion.Substring(1, 4));
                barcodeInfomation.CoordinateY = Convert.ToInt16(articalPostion.Substring(6, 4));
            }

            if (barCodeArray.Length < 8)
            {
                barcodeInfomation.Barcode = StaticParameterForSorter.NoRead;
                return true;
            }

            var sourceBarCode = barCodeArray[6];
            var barcodeconfig = ConfigurationManager.AppSettings["BarcodeConfig"];
            string barcodeRegex = string.Format(@"{0}", barcodeconfig);
            const string packageBarcodeRegex = @"VIP[A-Z]?\d{4,6}";
            const string orderBarcodeRegex = @"\d{14}";

            var barcode = Regex.Match(sourceBarCode, barcodeRegex);
            if (barcode.Success)
            {
                barcodeInfomation.Barcode = barcode.Value;
                if (barcode.NextMatch().Success)
                {
                    var theNexBarcode = barcode.NextMatch().Value;
                    if (barcodeInfomation.Barcode.Contains("VIP"))
                    {
                        barcodeInfomation.Barcode = barcode.Value;
                    }
                    else if (theNexBarcode.Contains("VIP"))
                    {
                        barcodeInfomation.Barcode = theNexBarcode;
                    }
                    else
                    {
                        //TODO TEST
                        barcodeInfomation.IfMultiBracode = true;
                    }
                }
            }
            var packageBarcode = Regex.Match(sourceBarCode, packageBarcodeRegex);
            if (packageBarcode.Success)
            {
                barcodeInfomation.PackageBarCode = packageBarcode.Value;
            }
            var orderBarcode = Regex.Match(sourceBarCode, orderBarcodeRegex);
            if (orderBarcode.Success)
            {
                barcodeInfomation.OrderBarCode = orderBarcode.Value;
            }
            return true;
        }
    }
}