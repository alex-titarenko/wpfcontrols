using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TAlex.WPF.Converters;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestClass]
    public class ColorToBrushConverterTest
    {
        [TestMethod]
        public void ConvertTest_Null()
        {
            ColorToBrushConverter target = new ColorToBrushConverter();
            SolidColorBrush actual = target.Convert(null, typeof(Brush), null, null) as SolidColorBrush;

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ConvertTest()
        {
            ColorToBrushConverter target = new ColorToBrushConverter();

            SolidColorBrush expected = Brushes.Orange;
            SolidColorBrush actual = target.Convert(Colors.Orange, typeof(Brush), null, null) as SolidColorBrush;

            Assert.AreEqual(expected.Color, actual.Color);
        }
    }
}
