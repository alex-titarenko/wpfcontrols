using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestFixture]
    public class NotEmptyStringToBoolConverterTest
    {
        #region Convert

        [Test]
        public void ConvertTest_Text()
        {
            NotEmptyStringToBooleanConverter target = new NotEmptyStringToBooleanConverter();

            bool expected = true;
            bool actual = (bool)target.Convert("Some text", typeof(bool), null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
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

        [Test]
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

        #endregion
    }
}
