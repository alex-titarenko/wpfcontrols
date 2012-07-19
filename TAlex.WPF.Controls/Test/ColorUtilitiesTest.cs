using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using TAlex.WPF.Media;


namespace TAlex.WPF.Test
{
    [TestClass]
    public class ColorUtilitiesTest
    {
        [TestMethod]
        public void GetHexCodeTest()
        {
            string expected = "#FFFF4500";
            string actual = ColorUtilities.GetHexCode(Colors.OrangeRed);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ColorToBgra32Test()
        {
            int expected;
            unchecked { expected = (int)0xFF98FB98; }
            int actual = ColorUtilities.ColorToBgra32(Colors.PaleGreen);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsKnownColorTest()
        {
            bool expected = true;
            string expectedName = "Pale Goldenrod";

            string actualName;
            bool actual = ColorUtilities.IsKnownColor(Colors.PaleGoldenrod, out actualName);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedName, actualName);

            expected = false;

            actualName = null;
            actual = ColorUtilities.IsKnownColor(Color.FromArgb(255, 35, 122, 108), out actualName);

            Assert.AreEqual(expected, actual);
            Assert.IsNull(actualName);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ParseColorTest_NullValue()
        {
            Color actual = ColorUtilities.ParseColor(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseColorTest_EmptyValue()
        {
            Color actual = ColorUtilities.ParseColor(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseColorTest_IncorrectFormat()
        {
            Color actual = ColorUtilities.ParseColor("Some String");
        }

        [TestMethod]
        public void ParseColorTest_HexValue()
        {
            Color expected = Colors.Turquoise;
            Color actual = ColorUtilities.ParseColor("#FF40E0D0");
            Assert.AreEqual(expected, actual);

            actual = ColorUtilities.ParseColor("#FF40E0D");
            Assert.AreEqual(Color.FromArgb(15, 244, 14, 13), actual);

            actual = ColorUtilities.ParseColor("#FF40E0");
            Assert.AreEqual(Color.FromArgb(255, 255, 64, 224), actual);

            actual = ColorUtilities.ParseColor("#FF40E");
            Assert.AreEqual(Color.FromArgb(255, 15, 244, 14), actual);

            actual = ColorUtilities.ParseColor("#FF40");
            Assert.AreEqual(Color.FromArgb(255, 0, 255, 64), actual);

            actual = ColorUtilities.ParseColor("#FF4");
            Assert.AreEqual(Color.FromArgb(255, 0, 15, 244), actual);

            actual = ColorUtilities.ParseColor("#FF");
            Assert.AreEqual(Color.FromArgb(255, 0, 0, 255), actual);

            actual = ColorUtilities.ParseColor("#F");
            Assert.AreEqual(Color.FromArgb(255, 0, 0, 15), actual);
        }

        [TestMethod]
        public void ParseColorTest_KnownName()
        {
            Color expected = Colors.OrangeRed;
            Color actual = ColorUtilities.ParseColor("OrangeRed");
            Assert.AreEqual(expected, actual);

            actual = ColorUtilities.ParseColor("ORANGE RED");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryParseColor()
        {
            Color expectedColor = Colors.Green;
            bool expected = true;

            Color actualColor;
            bool actual = ColorUtilities.TryParseColor("#FF008000", out actualColor);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedColor, actualColor);
        }

        [TestMethod]
        public void TryParseColor_IncorrectFormat()
        {
            Color expectedColor = Colors.Transparent;
            bool expected = false;

            Color actualColor;
            bool actual = ColorUtilities.TryParseColor("Some String", out actualColor);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedColor, actualColor);
        }

        [TestMethod]
        public void ColorToBgra32Test_FromChannels()
        {
            int expected;
            unchecked { expected = (int)0xFF98FB98; }
            Color c = Colors.PaleGreen;
            int actual = ColorUtilities.ColorToBgra32(c.R, c.G, c.B);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RgbToHsvTest()
        {
            Color rgbColor = Colors.Azure;

            HsvColor hsvColor = ColorUtilities.RgbToHsv(rgbColor);
            Assert.AreEqual(rgbColor, ColorUtilities.HsvToRgb(hsvColor));
        }

        [TestMethod]
        public void HsvToRgbTest()
        {
            HsvColor hsvColor = new HsvColor(180, 0.05882354003329282, 1);

            Color rgbColor = ColorUtilities.HsvToRgb(hsvColor);
            Assert.AreEqual(hsvColor, ColorUtilities.RgbToHsv(rgbColor));
        }

        [TestMethod]
        public void FuzzyColorEqualsRgbTest()
        {
            bool expected = true;

            Color color1 = Color.FromScRgb(1f, 0.1f, 0.5f, 1f);
            Color color2 = Color.FromScRgb(1f, 0.101f, 0.5f, 1f);
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FuzzyColorEqualsRgbTest_Unequal()
        {
            bool expected = false;

            Color color1 = Color.FromScRgb(1f, 0.1f, 0.5f, 1f);
            Color color2 = Color.FromScRgb(1f, 0.2f, 0.5f, 1f);
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FuzzyColorEqualsHsvTest()
        {
            bool expected = true;

            HsvColor color1 = new HsvColor(155, 0.5, 0.8);
            HsvColor color2 = new HsvColor(155.346, 0.504, 0.8);
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FuzzyColorEqualsHsvTest_Unequal()
        {
            bool expected = false;

            HsvColor color1 = new HsvColor(155, 0.5, 0.8);
            HsvColor color2 = new HsvColor(156, 0.5, 0.8);
            bool actual = ColorUtilities.FuzzyColorEquals(color1, color2);

            Assert.AreNotEqual(color1, color2);
            Assert.AreEqual(expected, actual);
        }
    }
}
