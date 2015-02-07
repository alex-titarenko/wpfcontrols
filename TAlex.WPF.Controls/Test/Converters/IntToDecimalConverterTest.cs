using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestFixture]
    public class IntToDecimalConverterTest
    {
        #region Convert

        [Test]
        public void ConvertTest()
        {
            Int32ToDecimalConverter target = new Int32ToDecimalConverter();

            decimal expected = 346;
            decimal actual = (decimal)target.Convert(346, typeof(Decimal), null, null);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ConvertBack

        [Test]
        public void ConvertBackTest()
        {
            Int32ToDecimalConverter target = new Int32ToDecimalConverter();

            int expected = 346;
            int actual = (int)target.ConvertBack(346M, typeof(int), null, null);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
