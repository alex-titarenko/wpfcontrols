using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
    [TestFixture]
    public class CamelTextToRegularTextConverterTests
    {
        protected CamelTextToRegularTextConverter target;

        [SetUp]
        public void SetUp()
        {
            target = new CamelTextToRegularTextConverter();
        }


        #region Convert

        [Test]
        public void Convert_CamelText_RegularText()
        {
            //arrange
            string expected = "Some Text";

            //action
            string actual = target.Convert("SomeText", typeof(string), null, null) as String;

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_Acronym_Acronym()
        {
            //arrange
            string expected = "IBM";

            //action
            string actual = target.Convert("IBM", typeof(string), null, null) as String;

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_MixedText_RegularTextWithAcronym()
        {
            //arrange
            string expected = "Learn WCF In Six Easy Months";

            //action
            string actual = target.Convert("LearnWCFInSixEasyMonths", typeof(string), null, null) as String;

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_AlphaNumericText_RegularText()
        {
            //arrange
            string expected = "P346 Sid";

            //action
            string actual = target.Convert("P346Sid", typeof(string), null, null) as String;

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
