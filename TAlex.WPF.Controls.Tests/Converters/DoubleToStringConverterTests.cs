using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
    [TestFixture]
    public class DoubleToStringConverterTests
    {
        protected DoubleToStringConverter Target;

        [SetUp]
        public void SetUp()
        {
            Target = new DoubleToStringConverter();
        }
        

        #region Convert

        [Test]
        public void Convert_DoubleNumber_AppropriateString()
        {
            //arrange
            string expected = "36.457";
            
            //action
            string actual = Target.Convert(36.457, typeof(String), null, CultureInfo.InvariantCulture) as String;

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ConvertBack

        [Test]
        public void ConvertBack_NumberString_AppropriateDoubleNumber()
        {
            //arrange
            double expected = 36.457;
            
            //action
            double actual = (double)Target.ConvertBack("36.457", typeof(Double), null, CultureInfo.InvariantCulture);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertBack_NotNumberString_ValidationResult()
        {
            //action
            object actual = Target.ConvertBack("test", typeof(Double), null, CultureInfo.InvariantCulture);

            //assert
            Assert.IsInstanceOf<ValidationResult>(actual);
        }

        #endregion
    }
}
