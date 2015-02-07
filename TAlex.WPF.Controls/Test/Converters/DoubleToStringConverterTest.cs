using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TAlex.WPF.Converters;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestClass]
    public class DoubleToStringConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            DoubleToStringConverter target = new DoubleToStringConverter();

            string expected = "36.457";
            string actual = target.Convert(36.457, typeof(String), null, CultureInfo.InvariantCulture) as String;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBack()
        {
            DoubleToStringConverter target = new DoubleToStringConverter();

            double expected = 36.457;
            double actual = (double)target.ConvertBack("36.457", typeof(Double), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBack_ErrorInput()
        {
            DoubleToStringConverter target = new DoubleToStringConverter();

            object actual = target.ConvertBack("test", typeof(Double), null, CultureInfo.InvariantCulture);
            Assert.IsInstanceOfType(actual, typeof(ValidationResult));
        }
    }
}
