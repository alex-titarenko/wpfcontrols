using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TAlex.WPF.Converters;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestClass]
    public class BooleanToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest_Null()
        {
            BooleanToVisibilityConverter target = new BooleanToVisibilityConverter();

            Visibility expected = Visibility.Collapsed;
            Visibility actual = (Visibility)target.Convert((bool?)null, typeof(Visibility), null, null);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_True()
        {
            BooleanToVisibilityConverter target = new BooleanToVisibilityConverter();

            Visibility expected = Visibility.Visible;
            Visibility actual = (Visibility)target.Convert(true, typeof(Visibility), null, null);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_FalseCollapsed()
        {
            BooleanToVisibilityConverter target = new BooleanToVisibilityConverter();

            Visibility expected = Visibility.Collapsed;
            Visibility actual = (Visibility)target.Convert(false, typeof(Visibility), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTest_FalseHidden()
        {
            BooleanToVisibilityConverter target = new BooleanToVisibilityConverter();
            target.UseHidden = true;

            Visibility expected = Visibility.Hidden;
            Visibility actual = (Visibility)target.Convert(false, typeof(Visibility), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBackTest_Visible()
        {
            BooleanToVisibilityConverter target = new BooleanToVisibilityConverter();

            bool expected = true;
            bool actual = (bool)target.ConvertBack(Visibility.Visible, typeof(bool), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBackTest_HiddenCollapsed()
        {
            BooleanToVisibilityConverter target = new BooleanToVisibilityConverter();

            bool expected = false;

            bool actual = (bool)target.ConvertBack(Visibility.Hidden, typeof(bool), null, null);
            Assert.AreEqual(expected, actual);

            actual = (bool)target.ConvertBack(Visibility.Collapsed, typeof(bool), null, null);
            Assert.AreEqual(expected, actual);
        }
    }
}
