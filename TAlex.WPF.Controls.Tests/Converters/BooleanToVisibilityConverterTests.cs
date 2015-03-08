using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
    [TestFixture]
    public class BooleanToVisibilityConverterTests
    {
        protected BooleanToVisibilityConverter Target;

        [SetUp]
        public void SetUp()
        {
            Target = new BooleanToVisibilityConverter();
        }


        #region Convert

        [Test]
        public void Convert_Null_Collapsed()
        {
            //arrange
            Visibility expected = Visibility.Collapsed;

            //action
            Visibility actual = (Visibility)Target.Convert((bool?)null, typeof(Visibility), null, null);
            
            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_True_Visible()
        {
            //arrange
            Visibility expected = Visibility.Visible;

            //action
            Visibility actual = (Visibility)Target.Convert(true, typeof(Visibility), null, null);
            
            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_False_Collapsed()
        {
            //arrange
            Visibility expected = Visibility.Collapsed;

            //action
            Visibility actual = (Visibility)Target.Convert(false, typeof(Visibility), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_FalseWithUseHidden_Hidden()
        {
            //arrange
            Target.UseHidden = true;
            Visibility expected = Visibility.Hidden;

            //action
            Visibility actual = (Visibility)Target.Convert(false, typeof(Visibility), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ConvertBack

        [Test]
        public void ConvertBack_Visible_True()
        {
            //arrange
            bool expected = true;

            //action
            bool actual = (bool)Target.ConvertBack(Visibility.Visible, typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertBack_HiddenOrCollapsed_False([Values(Visibility.Hidden, Visibility.Collapsed)]Visibility visibility)
        {
            //arrange
            bool expected = false;

            //action
            bool actual = (bool)Target.ConvertBack(visibility, typeof(bool), null, null);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
