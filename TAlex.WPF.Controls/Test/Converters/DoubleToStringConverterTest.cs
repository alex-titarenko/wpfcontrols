using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestFixture]
    public class DoubleToStringConverterTest
    {
        #region Convert

        [Test]
        public void ConvertTest()
        {
            DoubleToStringConverter target = new DoubleToStringConverter();

            string expected = "36.457";
            string actual = target.Convert(36.457, typeof(String), null, CultureInfo.InvariantCulture) as String;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ConvertBack

        [Test]
        public void ConvertBack()
        {
            DoubleToStringConverter target = new DoubleToStringConverter();

            double expected = 36.457;
            double actual = (double)target.ConvertBack("36.457", typeof(Double), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertBack_ErrorInput()
        {
            DoubleToStringConverter target = new DoubleToStringConverter();

            object actual = target.ConvertBack("test", typeof(Double), null, CultureInfo.InvariantCulture);
            Assert.IsInstanceOf<ValidationResult>(actual);
        }

        #endregion
    }
}
