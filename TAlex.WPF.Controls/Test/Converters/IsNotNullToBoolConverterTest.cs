using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Test.Converters
{
    /// <summary>
    /// Summary description for NotNullToBoolConverter
    /// </summary>
    [TestFixture]
    public class IsNotNullToBoolConverterTest
    {
        #region Convert

        [Test]
        public void ConvertTest_NotNull()
        {
            IsNotNullToBooleanConverter target = new IsNotNullToBooleanConverter();
            bool expected = true;
            bool actual = (bool)target.Convert("Some string", typeof(Boolean), null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertTest_Null()
        {
            IsNotNullToBooleanConverter target = new IsNotNullToBooleanConverter();
            bool expected = false;
            bool actual = (bool)target.Convert(null, typeof(Boolean), null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertTest_IsReversed()
        {
            IsNotNullToBooleanConverter target = new IsNotNullToBooleanConverter();
            target.IsReversed = true;

            bool expected = true;
            bool actual = (bool)target.Convert(null, typeof(Boolean), null, null);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = (bool)target.Convert(5, typeof(Boolean), null, null);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
