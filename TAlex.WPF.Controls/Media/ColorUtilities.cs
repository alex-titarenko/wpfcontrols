using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Windows.Media;


namespace TAlex.WPF.Media
{
    /// <summary>
    /// Represents frequently used tools for working with color.
    /// </summary>
    public static class ColorUtilities
    {
        #region Fields

        private static readonly ICollection<ColorItem> KnownColorItems;

        private static readonly IDictionary<string, Color> KnownColors;

        #endregion

        #region Constructors

        static ColorUtilities()
        {
            KnownColorItems = GetKnownColorItems();

            KnownColors = new Dictionary<string, Color>();
            foreach (ColorItem colorItem in KnownColorItems)
            {
                KnownColors.Add(GetStandardColorName(colorItem.Name), colorItem.Color);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representing a <see cref="System.Windows.Media.Color"/> written as hex code.
        /// </summary>
        /// <param name="c">A <see cref="System.Windows.Media.Color"/> value.</param>
        /// <returns><see cref="System.String"/> representing <paramref name="c"/> written as hex code.</returns>
        public static string GetHexCode(Color c)
        {
            return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts the string representation of a color to its <see cref="System.Windows.Media.Color"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <returns>A <see cref="System.Windows.Media.Color"/> equivalent to the color contained in s.</returns>
        /// <exception cref="System.NullReferenceException">s is null value.</exception>
        /// <exception cref="System.FormatException">s is empty string or written in incorrect format.</exception>
        public static Color ParseColor(string s)
        {
            s = s.Trim();
            if (s.StartsWith("#")) s = s.Substring(1);

            if (String.IsNullOrEmpty(s))
                throw new FormatException();

            // Parse color by known name
            Color c;
            if (KnownColors.TryGetValue(GetStandardColorName(s), out c))
            {
                return c;
            }

            // Parse color by hex value
            int len = s.Length;

            byte a = 255, r = 0, g = 0, b = 0;

            switch (len)
            {
                case 1:
                case 2:
                    b = byte.Parse(s, NumberStyles.AllowHexSpecifier);
                    break;

                case 3:
                case 4:
                    g = byte.Parse(s.Substring(0, len - 2), NumberStyles.AllowHexSpecifier);
                    b = byte.Parse(s.Substring(len - 2), NumberStyles.AllowHexSpecifier);
                    break;

                case 5:
                case 6:
                    r = byte.Parse(s.Substring(0, len - 4), NumberStyles.AllowHexSpecifier);
                    g = byte.Parse(s.Substring(len - 4, 2), NumberStyles.AllowHexSpecifier);
                    b = byte.Parse(s.Substring(len - 2), NumberStyles.AllowHexSpecifier);
                    break;

                case 7:
                case 8:
                    a = byte.Parse(s.Substring(0, len - 6), NumberStyles.AllowHexSpecifier);
                    r = byte.Parse(s.Substring(len - 6, 2), NumberStyles.AllowHexSpecifier);
                    g = byte.Parse(s.Substring(len - 4, 2), NumberStyles.AllowHexSpecifier);
                    b = byte.Parse(s.Substring(len - 2), NumberStyles.AllowHexSpecifier);
                    break;

                default:
                    throw new FormatException();
            }

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Converts the string representation of a color to its <see cref="System.Windows.Media.Color"/> equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">The string representation of the color to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="System.Windows.Media.Color"/> that is equivalent
        /// to the color value contained in s, if the conversion succeeded, or transporent color
        /// if the conversion failed. The conversion fails if the s parameter is null,
        /// is an empty string, or does not contain a valid string representation of
        /// a color. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParseColor(string s, out Color result)
        {
            try
            {
                result = ParseColor(s);
                return true;
            }
            catch (FormatException)
            {
                result = Colors.Transparent;
                return false;
            }
        }

        /// <summary>
        /// Converts a <see cref="System.Windows.Media.Color"/> value to Int32.
        /// </summary>
        /// <param name="color">A <see cref="System.Windows.Media.Color"/> value.</param>
        /// <returns>Int32 value converted from <paramref name="color"/>.</returns>
        public static int ColorToBgra32(Color color)
        {
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }

        /// <summary>
        /// Converts a color channels to a Int32 color value.
        /// </summary>
        /// <param name="r">An integer value representing a red channel of color.</param>
        /// <param name="g">An integer value representing a green channel of color.</param>
        /// <param name="b">An integer value representing a blue channel of color.</param>
        /// <returns>Int32 color value converted from color channels.</returns>
        public static int ColorToBgra32(int r, int g, int b)
        {
            return (255 << 24) | (r << 16) | (g << 8) | b;
        }

        /// <summary>
        /// Converts rgb color to hsv color.
        /// </summary>
        /// <param name="rgbColor">A <see cref="System.Windows.Media.Color"/> value.</param>
        /// <returns><see cref="TAlex.WPF.Media.HsvColor"/> value converted from <paramref name="rgbColor"/>.</returns>
        public static HsvColor RgbToHsv(Color rgbColor)
        {
            double a = rgbColor.ScA;
            double r = ScRgbTosRgb(rgbColor.ScR);
            double g = ScRgbTosRgb(rgbColor.ScG);
            double b = ScRgbTosRgb(rgbColor.ScB);

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;

            // Hue
            double h = 0;
            if (max == min)
            {
                h = 0;
            }
            else if (max == r)
            {
                h = 60 * (g - b) / delta;
                if (g < b) h += 360;
            }
            else if (max == g)
            {
                h = 60 * (b - r) / delta + 120;
            }
            else if (max == b)
            {
                h = 60 * (r - g) / delta + 240;
            }

            // Saturation
            double s = (max == 0) ? 0 : 1 - min / max;

            // Value (brightness)
            double v = max;

            return new HsvColor(a, h, s, v);
        }

        /// <summary>
        /// Converts hsv color to rgb color.
        /// </summary>
        /// <param name="hsvColor">A <see cref="TAlex.WPF.Media.HsvColor"/> value</param>
        /// <returns>A <see cref="System.Windows.Media.Color"/> value converted from <paramref name="hsvColor"/>.</returns>
        public static Color HsvToRgb(HsvColor hsvColor)
        {
            double h = hsvColor.H;
            double s = hsvColor.S;
            double v = hsvColor.V;

            double r = 0;
            double g = 0;
            double b = 0;

            if (hsvColor.S == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                int hi = (int)Math.Truncate(h / 60) % 6;
                double f = h / 60 - Math.Truncate(h / 60);

                double p = v * (1 - s);
                double q = v * (1 - f * s);
                double t = v * (1 - (1 - f) * s);

                switch (hi)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }

            return Color.FromScRgb((float)hsvColor.A, sRgbToScRgb((float)r), sRgbToScRgb((float)g), sRgbToScRgb((float)b));
        }

        /// <summary>
        /// Returns a hue brush with specified saturation and brightness values.
        /// </summary>
        /// <param name="s">A floating-point number representing a saturation for the hue brush.</param>
        /// <param name="v">A floating-point number representing a value (brightness) for the hue brush.</param>
        /// <returns><see cref="System.Windows.Media.Brush"/> representing of hue colors.</returns>
        public static Brush GetHueBrash(double s, double v)
        {
            LinearGradientBrush br = new LinearGradientBrush();

            br.GradientStops.Add(new GradientStop(new HsvColor(0, s, v).ToRgb(), 0.0));
            br.GradientStops.Add(new GradientStop(new HsvColor(60, s, v).ToRgb(), 1.0 / 6.0));
            br.GradientStops.Add(new GradientStop(new HsvColor(120, s, v).ToRgb(), 2.0 / 6.0));
            br.GradientStops.Add(new GradientStop(new HsvColor(180, s, v).ToRgb(), 0.5));
            br.GradientStops.Add(new GradientStop(new HsvColor(240, s, v).ToRgb(), 4.0 / 6.0));
            br.GradientStops.Add(new GradientStop(new HsvColor(300, s, v).ToRgb(), 5.0 / 6.0));
            br.GradientStops.Add(new GradientStop(new HsvColor(360, s, v).ToRgb(), 1.0));

            return br;
        }

        /// <summary>
        /// Tests whether two <see cref="System.Windows.Media.Color"/> structures are identical by integer values of color channels.
        /// </summary>
        /// <param name="c1">The first <see cref="System.Windows.Media.Color"/> structure to compare.</param>
        /// <param name="c2">The second <see cref="System.Windows.Media.Color"/> structure to compare.</param>
        /// <returns>true if <paramref name="c1"/> and <paramref name="c2"/> are fuzzy identical; otherwise, false.</returns>
        public static bool FuzzyColorEquals(Color c1, Color c2)
        {
            return c1.A == c2.A && c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }

        /// <summary>
        /// Tests whether two <see cref="TAlex.WPF.Media.HsvColor"/> structures are identical by integer values of color components.
        /// </summary>
        /// <param name="c1">The first <see cref="TAlex.WPF.Media.HsvColor"/> structure to compare.</param>
        /// <param name="c2">The second <see cref="TAlex.WPF.Media.HsvColor"/> structure to compare.</param>
        /// <returns>true if <paramref name="c1"/> and <paramref name="c2"/> are fuzzy identical; otherwise, false.</returns>
        public static bool FuzzyColorEquals(HsvColor c1, HsvColor c2)
        {
            return c1.IntA == c2.IntA && c1.IntH == c2.IntH && c1.IntS == c2.IntS && c1.IntV == c2.IntV;
        }

        /// <summary>
        /// Returns a value indicating whether the specified color is a predefined and when true sets the name of the color.
        /// </summary>
        /// <param name="color">A <see cref="System.Windows.Media.Color"/> value to inspecting.</param>
        /// <param name="name">The name of the predefined color.</param>
        /// <returns>true if <paramref name="color"/> is predefined color; otherwise, false.</returns>
        public static bool IsKnownColor(Color color, out string name)
        {
            foreach (ColorItem item in KnownColorItems)
            {
                if (Color.AreClose(item.Color, color))
                {
                    name = item.Name;
                    return true;
                }
            }

            name = null;
            return false;
        }


        internal static double ClipValue(double value, double min, double max)
        {
            if (double.IsNaN(value) || double.IsNegativeInfinity(value) || value < min)
                return min;
            else if (double.IsPositiveInfinity(value) || value > max)
                return max;
            else
                return value;
        }

        internal static double ScRgbTosRgb(float val)
        {
            if (val <= 0.0)
            {
                return 0;
            }
            if (val <= 0.0031308)
            {
                return val * 12.92;
            }
            if (val < 1.0)
            {
                return 1.055 * Math.Pow(val, 1 / 2.4) - 0.055;
            }
            return 1.0;
        }

        internal static float sRgbToScRgb(float val)
        {
            if (val <= 0.0)
            {
                return 0f;
            }
            if (val <= 0.04045)
            {
                return (val / 12.92f);
            }
            if (val < 1f)
            {
                return (float)Math.Pow((val + 0.055) / 1.055, 2.4);
            }
            return 1f;
        }

        internal static ICollection<ColorItem> GetKnownColorItems()
        {
            IList<ColorItem> knownColorItems = new List<ColorItem>();

            foreach (PropertyInfo property in typeof(Colors).GetProperties())
            {
                string name = property.Name;
                Color color = (Color)property.GetValue(null, null);

                knownColorItems.Add(GetColorItem(color, name));
            }

            return knownColorItems;
        }

        internal static ICollection<ColorItem> GetKnownColorsWithoutTransparent()
        {
            List<ColorItem> knownColors = new List<ColorItem>();

            foreach (PropertyInfo property in typeof(Colors).GetProperties())
            {
                string name = property.Name;
                Color color = (Color)property.GetValue(null, null);

                if (color != Colors.Transparent)
                    knownColors.Add(GetColorItem(color, name));
            }

            return knownColors;
        }

        internal static ColorItem GetColorItem(Color color, string name)
        {
            ColorItem colorItem = new ColorItem();
            colorItem.Name = NiceColorName(name);
            colorItem.Color = color;

            return colorItem;
        }

        internal static string NiceColorName(string name)
        {
            if (name.StartsWith("#"))
            {
                return name;
            }

            StringBuilder sb = new StringBuilder();

            foreach (char ch in name)
            {
                if (Char.IsUpper(ch))
                    sb.Append(' ');

                sb.Append(ch);
            }

            return sb.ToString().Trim();
        }

        internal static string GetStandardColorName(string name)
        {
            return name.Replace(" ", "").ToUpper();
        }

        #endregion
    }
}
