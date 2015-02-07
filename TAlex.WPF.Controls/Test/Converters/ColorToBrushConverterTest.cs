using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Test.Converters
{
    [TestFixture]
    public class ColorToBrushConverterTest
    {
        #region Convert

        [Test]
        public void ConvertTest_Null()
        {
            ColorToBrushConverter target = new ColorToBrushConverter();
            SolidColorBrush actual = target.Convert(null, typeof(Brush), null, null) as SolidColorBrush;

            Assert.IsNull(actual);
        }

        [Test]
        public void ConvertTest()
        {
            ColorToBrushConverter target = new ColorToBrushConverter();

            SolidColorBrush expected = Brushes.Orange;
            SolidColorBrush actual = target.Convert(Colors.Orange, typeof(Brush), null, null) as SolidColorBrush;

            Assert.AreEqual(expected.Color, actual.Color);
        }

        #endregion
    }
}
