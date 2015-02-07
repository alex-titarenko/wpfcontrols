using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TAlex.WPF.Converters;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestClass]
    public class IntToDecimalConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            Int32ToDecimalConverter target = new Int32ToDecimalConverter();

            decimal expected = 346;
            decimal actual = (decimal)target.Convert(346, typeof(Decimal), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            Int32ToDecimalConverter target = new Int32ToDecimalConverter();

            int expected = 346;
            int actual = (int)target.ConvertBack(346M, typeof(int), null, null);

            Assert.AreEqual(expected, actual);
        }
    }
}
