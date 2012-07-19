using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TAlex.WPF.Converters;


namespace TAlex.WPF.Test
{
    [TestClass]
    public class NotEmptyStringToBoolConverterTest
    {
        [TestMethod]
        public void ConvertTest_Text()
        {
            NotEmptyStringToBooleanConverter target = new NotEmptyStringToBooleanConverter();

            bool expected = true;
            bool actual = (bool)target.Convert("Some text", typeof(bool), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_WhiteSpaces()
        {
            NotEmptyStringToBooleanConverter target = new NotEmptyStringToBooleanConverter();

            bool expected = false;

            bool actual = (bool)target.Convert(String.Empty, typeof(bool), null, null);
            Assert.AreEqual(expected, actual);

            actual = (bool)target.Convert(" \t  ", typeof(bool), null, null);
            Assert.AreEqual(expected, actual);

            actual = (bool)target.Convert(null, typeof(bool), null, null);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_IsReversed()
        {
            NotEmptyStringToBooleanConverter target = new NotEmptyStringToBooleanConverter();
            target.IsReversed = true;

            bool expected = true;
            bool actual = (bool)target.Convert(String.Empty, typeof(bool), null, null);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = (bool)target.Convert("Universe", typeof(bool), null, null);
            Assert.AreEqual(expected, actual);
        }
    }
}
