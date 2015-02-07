using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestFixture]
    public class NotEmptyStringToBoolConverterTests
    {
        NotEmptyStringToBooleanConverter target;

        [SetUp]
        public void SetUp()
        {
            target = new NotEmptyStringToBooleanConverter();
        }


        #region Convert

        [Test]
        public void ConvertTest_Text()
        {
            //arrange
            bool expected = true;

            //action
            bool actual = (bool)target.Convert("Some text", typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("")]
        [TestCase(" \t  ")]
        [TestCase(null)]
        public void ConvertTest_WhiteSpaces(string value)
        {
            //arrange
            bool expected = false;

            //action
            bool actual = (bool)target.Convert(value, typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("", true)]
        [TestCase("Universe", false)]
        public void ConvertTest_IsReversed(string value, bool expected)
        {
            //arrange
            target.IsReversed = true;

            //action
            bool actual = (bool)target.Convert(value, typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
