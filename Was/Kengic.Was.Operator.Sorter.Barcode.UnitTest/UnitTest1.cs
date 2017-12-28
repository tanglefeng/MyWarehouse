using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Kengic.Was.Operator.Sorter.Barcode.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
          
            var barcodeconfig = ConfigurationManager.AppSettings["BarcodeConfig"];
            string barcodeRegex = string.Format(@"{0}", barcodeconfig);

            Assert.AreEqual(barcodeRegex, @"(P_[A-Z]{4}|VIP_[A-Z]{2})\d{10}|BOXNH\d{14}|PJ\d{13}|NBJCBS\d{10}");
        }
    }
}
