using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TAlex.WPF.Converters;


namespace TAlex.WPF.Test
{
    [TestClass]
    public class CamelTextToRegularTextConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            CamelTextToRegularTextConverter target = new CamelTextToRegularTextConverter();

            string expected = "Some Text";
            string actual = target.Convert("SomeText", typeof(string), null, null) as String;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_Acronym()
        {
            CamelTextToRegularTextConverter target = new CamelTextToRegularTextConverter();

            string expected = "IBM";
            string actual = target.Convert("IBM", typeof(string), null, null) as String;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_Combined()
        {
            CamelTextToRegularTextConverter target = new CamelTextToRegularTextConverter();

            string expected = "Learn WCF In Six Easy Months";
            string actual = target.Convert("LearnWCFInSixEasyMonths", typeof(string), null, null) as String;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_AlphaNumeric()
        {
            CamelTextToRegularTextConverter target = new CamelTextToRegularTextConverter();

            string expected = "P346 Sid";
            string actual = target.Convert("P346Sid", typeof(string), null, null) as String;

            Assert.AreEqual(expected, actual);
        }
    }
}
