using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
    [TestFixture]
    public class NotEmptyStringToBoolConverterTests
    {
        protected NotEmptyStringToBooleanConverter Target;

        [SetUp]
        public void SetUp()
        {
            Target = new NotEmptyStringToBooleanConverter();
        }


        #region Convert

        [Test]
        public void Convert_SomeText_True()
        {
            //arrange
            bool expected = true;

            //action
            bool actual = (bool)Target.Convert("Some text", typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("")]
        [TestCase(" \t  ")]
        [TestCase(null)]
        public void Convert_Whitespaces_False(string value)
        {
            //arrange
            bool expected = false;

            //action
            bool actual = (bool)Target.Convert(value, typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_IsReversedWithEmptyString_True()
        {
            //arrange
            Target.IsReversed = true;

            //action
            bool actual = (bool)Target.Convert(String.Empty, typeof(bool), null, null);

            //assert
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Convert_IsReversedWithSomeText_False()
        {
            //arrange
            Target.IsReversed = true;

            //action
            bool actual = (bool)Target.Convert("Universe", typeof(bool), null, null);

            //assert
            Assert.AreEqual(false, actual);
        }

        #endregion
    }
}
