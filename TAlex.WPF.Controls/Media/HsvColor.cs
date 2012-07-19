using System;
using System.Windows.Media;


namespace TAlex.WPF.Media
{
    /// <summary>
    /// Describes a color in terms of alpha, hue, saturation, and value (brightness).
    /// </summary>
    public struct HsvColor
    {
        #region Fields

        /// <summary>
        /// Represents the maximum value of the hue of the color. This field is constant.
        /// </summary>
        public const double MaxHueValue = 360.0;

        private double _a;
        private double _h;
        private double _s;
        private double _v;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the alpha channel of the color. Value ranges from 0 to 1.
        /// </summary>
        public double A
        {
            get
            {
                return _a;
            }

            set
            {
                _a = ColorUtilities.ClipValue(value, 0, 1);
            }
        }

        /// <summary>
        /// Gets or sets the hue of the color. Value ranges from 0 to 360.
        /// </summary>
        public double H
        {
            get
            {
                return _h;
            }

            set
            {
                _h = ColorUtilities.ClipValue(value, 0, MaxHueValue);
            }
        }

        /// <summary>
        /// Gets or sets the saturation of the color. Value ranges from 0 to 1.
        /// </summary>
        public double S
        {
            get
            {
                return _s;
            }

            set
            {
                _s = ColorUtilities.ClipValue(value, 0, 1);
            }
        }

        /// <summary>
        /// Gets or sets the value (brightness) of the color. Value ranges from 0 to 1.
        /// </summary>
        public double V
        {
            get
            {
                return _v;
            }

            set
            {
                _v = ColorUtilities.ClipValue(value, 0, 1);
            }
        }

        /// <summary>
        /// Gets or sets integer value of the alpha channel of the color. Value ranges from 0 to 100.
        /// </summary>
        public int IntA
        {
            get
            {
                return (int)Math.Ceiling(_a * 100);
            }

            set
            {
                _a = ColorUtilities.ClipValue(value / 100.0, 0, 1);
            }
        }

        /// <summary>
        /// Gets or sets integer value of the hue of the color. Value ranges from 0 to 360.
        /// </summary>
        public int IntH
        {
            get
            {
                int value = Convert.ToInt32(_h);

                if (value == MaxHueValue)
                    return 0;
                else
                    return value;
            }

            set
            {
                _h = ColorUtilities.ClipValue(value, 0, MaxHueValue);
            }
        }

        /// <summary>
        /// Gets or sets integer value of the saturation of the color. Value ranges from 0 to 100.
        /// </summary>
        public int IntS
        {
            get
            {
                return Convert.ToInt32(_s * 100);
            }

            set
            {
                _s = ColorUtilities.ClipValue(value / 100.0, 0, 1);
            }
        }

        /// <summary>
        /// Gets or sets integer value of the value (brightness) of the color. Value ranges from 0 to 100.
        /// </summary>
        public int IntV
        {
            get
            {
                return Convert.ToInt32(_v * 100);
            }

            set
            {
                _v = ColorUtilities.ClipValue(value / 100.0, 0, 1);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Media.HsvColor"/> structure.
        /// </summary>
        /// <param name="hue">A System.Double representing the hue of the new color.</param>
        /// <param name="saturation">A System.Double representing the saturation of the new color.</param>
        /// <param name="value">A System.Double representing the value (brightness) of the new color.</param>
        public HsvColor(double hue, double saturation, double value)
            : this(1, hue, saturation, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Media.HsvColor"/> structure.
        /// </summary>
        /// <param name="alpha">A System.Double representing the alpha channel of the new color.</param>
        /// <param name="hue">A System.Double representing the hue of the new color.</param>
        /// <param name="saturation">A System.Double representing the saturation of the new color.</param>
        /// <param name="value">A System.Double representing the value (brightness) of the new color.</param>
        public HsvColor(double alpha, double hue, double saturation, double value)
        {
            _a = ColorUtilities.ClipValue(alpha, 0, 1);
            _h = ColorUtilities.ClipValue(hue, 0, MaxHueValue);
            _s = ColorUtilities.ClipValue(saturation, 0, 1);
            _v = ColorUtilities.ClipValue(value, 0, 1);
        }

        #endregion

        #region Methods

        #region Dynamics

        /// <summary>
        /// Gets the scRGB of this HsvColor structure.
        /// </summary>
        /// <returns>A <see cref="System.Windows.Media.Color"/> converted from this instance.</returns>
        public Color ToRgb()
        {
            return ColorUtilities.HsvToRgb(this);
        }

        /// <summary>
        /// Creates a string representation of the color using the HSV channels.
        /// </summary>
        /// <returns>The string representation of the color.</returns>
        public override string ToString()
        {
            return String.Format("(h: {0}; s: {1}; v: {2})", H, S, V);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>true if obj is an instance of HsvColor and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is HsvColor)
            {
                return ((HsvColor)obj) == this;
            }

            return false;
        }

        /// <summary>
        /// Gets a hash code for the current HsvColor structure.
        /// </summary>
        /// <returns>A hash code for the current HsvColor structure.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion

        #region Statics

        /// <summary>
        /// Creates a new HsvColor structure by using the ScRGB alpha channel and color channel values.
        /// </summary>
        /// <param name="rgbColor">A <see cref="System.Windows.Media.Color"/> to be converted.</param>
        /// <returns>A HsvColor structure with the specified values.</returns>
        public static HsvColor FromRgb(Color rgbColor)
        {
            return ColorUtilities.RgbToHsv(rgbColor);
        }

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Tests whether two HsvColor structures are identical. 
        /// </summary>
        /// <param name="c1">The first HsvColor structure to compare.</param>
        /// <param name="c2">The second HsvColor structure to compare.</param>
        /// <returns>true if c1 and c2 are exactly identical; otherwise, false.</returns>
        public static bool operator ==(HsvColor c1, HsvColor c2)
        {
            return c1._a == c2._a && c1._h == c2._h && c1._s == c2._s && c1._v == c2._v;
        }

        /// <summary>
        /// Tests whether two HsvColor structures are not identical.
        /// </summary>
        /// <param name="c1">The first HsvColor structure to compare.</param>
        /// <param name="c2">The second HsvColor structure to compare.</param>
        /// <returns>true if c1 and c2 are not equal; otherwise, false.</returns>
        public static bool operator !=(HsvColor c1, HsvColor c2)
        {
            return !(c1 == c2);
        }

        #endregion
    }
}
