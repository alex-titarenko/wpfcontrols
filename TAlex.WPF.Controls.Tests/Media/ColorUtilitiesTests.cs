using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TAlex.WPF.Media;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Media
{
    [TestFixture]
    public class ColorUtilitiesTests
    {
        #region GetHexCode

        [Test]
        public void GetHexCode_Color_ColorString()
        {
            //arrange
            string expected = "#FFFF4500";
            
            //action
            string actual = ColorUtilities.GetHexCode(Colors.OrangeRed);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ColorToBgra32

        [Test]
        public void ColorToBgra32_Color_IntColor()
        {
            //arrange
            int expected;
            unchecked { expected = (int)0xFF98FB98; }

            //action
            int actual = ColorUtilities.ColorToBgra32(Colors.PaleGreen);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region IsKnownColor

        [Test]
        public void IsKnownColorTest_KnownColor_True()
        {
            //arrange
            bool expected = true;
            string expectedName = "Pale Goldenrod";
            string actualName;

            //action
            bool actual = ColorUtilities.IsKnownColor(Colors.PaleGoldenrod, out actualName);

            //assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void IsKnownColorTest_NotKnownColor_False()
        {
            //arrange
            bool expected = false;
            string actualName = null;

            //action
            bool actual = ColorUtilities.IsKnownColor(Color.FromArgb(255, 35, 122, 108), out actualName);

            //assert
            Assert.AreEqual(expected, actual);
            Assert.IsNull(actualName);
        }

        #endregion

        #region ParseColor

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ParseColorTest_Null_ThrowException()
        {
            //action
            Color actual = ColorUtilities.ParseColor(null);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void ParseColorTest_EmptyString_ThrowException()
        {
            //action
            Color actual = ColorUtilities.ParseColor(String.Empty);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void ParseColorTest_IncorrectFormat_ThrowException()
        {
            //action
            Color actual = ColorUtilities.ParseColor("Some String");
        }

        [TestCase("#FF40E0D0", 255, 64, 224, 208)]
        [TestCase("#FF40E0D", 15, 244, 14, 13)]
        [TestCase("#FF40E0", 255, 255, 64, 224)]
        [TestCase("#FF40E", 255, 15, 244, 14)]
        [TestCase("#FF40", 255, 0, 255, 64)]
        [TestCase("#FF4", 255, 0, 15, 244)]
        [TestCase("#FF", 255, 0, 0, 255)]
        [TestCase("#F", 255, 0, 0, 15)]
        public void ParseColorTest_HexValueString_Color(string s, byte a, byte r, byte g, byte b)
        {
            //arrange
            Color expected = Color.FromArgb(a, r, g, b);
            
            //action
            Color actual = ColorUtilities.ParseColor(s);
            
            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("OrangeRed", 255, 255, 69, 0)]
        [TestCase("ORANGE RED", 255, 255, 69, 0)]
        public void ParseColorTest_KnownNameColorString_Color(string s, byte a, byte r, byte g, byte b)
        {
            //arrange
            Color expected = Color.FromArgb(a, r, g, b);
            
            //action
            Color actual = ColorUtilities.ParseColor(s);
            
            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region TryParseColor

        [Test]
        public void TryParseColor_HexColorString_TrueAndColor()
        {
            //arrange
            Color expectedColor = Colors.Green;
            bool expected = true;
            Color actualColor;

            //action
            bool actual = ColorUtilities.TryParseColor("#FF008000", out actualColor);

            //assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedColor, actualColor);
        }

        [Test]
        public void TryParseColor_IncorrectFormatString_FalseAndTransparentColor()
        {
            //arrange
            Color expectedColor = Colors.Transparent;
            bool expected = false;
            Color actualColor;

            //action
            bool actual = ColorUtilities.TryParseColor("Some String", out actualColor);

            //assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedColor, actualColor);
        }

        #endregion

        #region ColorToBgra32

        [Test]
        public void ColorToBgra32Test_ColorChannelValues_BgraIntColor()
        {
            //arrange
            int expected;
            unchecked { expected = (int)0xFF98FB98; }
            Color c = Colors.PaleGreen;

            //action
            int actual = ColorUtilities.ColorToBgra32(c.R, c.G, c.B);

            //assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region RgbToHsv

        [Test]
        public void RgbToHsv_RgbColor_HsvColor()
        {
            //arrange
            Color rgbColor = Colors.Azure;

            //action
            HsvColor hsvColor = ColorUtilities.RgbToHsv(rgbColor);

            //assert
            Assert.AreEqual(rgbColor, ColorUtilities.HsvToRgb(hsvColor));
        }

        #endregion

        #region HsvToRgb

        [Test]
        public void HsvToRgb_HsvColor_RgbColor()
        {
            //arrange
            HsvColor hsvColor = new HsvColor(180, 0.05882354003329282, 1);

            //action
            Color rgbColor = ColorUtilities.HsvToRgb(hsvColor);

            //assert
            Assert.AreEqual(hsvColor, ColorUtilities.RgbToHsv(rgbColor));
        }

        #endregion

        #region FuzzyColorEquals

        [Test]
        public void FuzzyColorEquals_RgbFuzzyEqualColors_True()
        {
            //arrange
            bool expected = true;
            Color color1 = Color.FromScRgb(1f, 0.1f, 0.5f, 1f);
            Color color2 = Color.FromScRgb(1f, 0.101f, 0.5f, 1f);
            
            //action
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            //assert
            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FuzzyColorEquals_RgbFuzzyUnequalColors_False()
        {
            //arrange
            bool expected = false;
            Color color1 = Color.FromScRgb(1f, 0.1f, 0.5f, 1f);
            Color color2 = Color.FromScRgb(1f, 0.2f, 0.5f, 1f);

            //action
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            //assert
            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FuzzyColorEquals_HsvFuzzyEqualColors_True()
        {
            //arrange
            bool expected = true;
            HsvColor color1 = new HsvColor(155, 0.5, 0.8);
            HsvColor color2 = new HsvColor(155.346, 0.504, 0.8);

            //action
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            //assert
            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FuzzyColorEquals_HsvFuzzyUnequalColors_False()
        {
            //arrange
            bool expected = false;
            HsvColor color1 = new HsvColor(155, 0.5, 0.8);
            HsvColor color2 = new HsvColor(156, 0.5, 0.8);

            //action
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            //assert
            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
