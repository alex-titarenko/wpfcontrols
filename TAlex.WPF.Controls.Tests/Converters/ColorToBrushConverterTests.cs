using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TAlex.WPF.Converters;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Converters
{
    [TestFixture]
    public class ColorToBrushConverterTests
    {
        protected ColorToBrushConverter Target;

        [SetUp]
        public void SetUp()
        {
            Target = new ColorToBrushConverter();
        }


        #region Convert

        [Test]
        public void Convert_Null_Null()
        {            
            //action
            SolidColorBrush actual = Target.Convert(null, typeof(Brush), null, null) as SolidColorBrush;

            //assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Convert_Color_AppropriateBrush()
        {
            //arrange
            SolidColorBrush expected = Brushes.Orange;

            //action
            SolidColorBrush actual = Target.Convert(Colors.Orange, typeof(Brush), null, null) as SolidColorBrush;

            //assert
            Assert.AreEqual(expected.Color, actual.Color);
        }

        #endregion
    }
}
