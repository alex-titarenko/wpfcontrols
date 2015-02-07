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
    public class IsNotNullToBoolConverterTests
    {
        protected IsNotNullToBooleanConverter Target;

        [SetUp]
        public void SetUp()
        {
            Target = new IsNotNullToBooleanConverter();
        }


        #region Convert

        [Test]
        public void ConvertTest_NotNull()
        {
            //arrange
            bool expected = true;

            //action
            bool actual = (bool)Target.Convert("Some string", typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertTest_Null()
        {
            //arrange
            bool expected = false;

            //action
            bool actual = (bool)Target.Convert(null, typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, true)]
        [TestCase(5, false)]
        public void ConvertTest_IsReversed(object value, bool expected)
        {
            //arrange
            Target.IsReversed = true;

            //action
            bool actual = (bool)Target.Convert(value, typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
