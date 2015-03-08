using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
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
        public void Convert_SomeString_True()
        {
            //arrange
            bool expected = true;

            //action
            bool actual = (bool)Target.Convert("Some string", typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_Null_False()
        {
            //arrange
            bool expected = false;

            //action
            bool actual = (bool)Target.Convert(null, typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_IsReversedWithNull_True()
        {
            //arrange
            Target.IsReversed = true;

            //action
            bool actual = (bool)Target.Convert(null, typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Convert_IsReversedWithNumber_False()
        {
            //arrange
            Target.IsReversed = true;

            //action
            bool actual = (bool)Target.Convert(5, typeof(Boolean), null, null);

            //assert
            Assert.AreEqual(false, actual);
        }

        #endregion
    }
}
