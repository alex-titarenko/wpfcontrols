using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
    [TestFixture]
    public class IntToDecimalConverterTests
    {
        protected Int32ToDecimalConverter Target;

        [SetUp]
        public void SetUp()
        {
            Target = new Int32ToDecimalConverter();
        }
        

        #region Convert

        [Test]
        public void Convert_IntNumber_AppropriateDecimal()
        {
            //arrange
            decimal expected = 346;

            //action
            decimal actual = (decimal)Target.Convert(346, typeof(Decimal), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ConvertBack

        [Test]
        public void ConvertBack_Decimal_AppropriateIntNumber()
        {
            //arrange
            int expected = 346;

            //action
            int actual = (int)Target.ConvertBack(346M, typeof(int), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
