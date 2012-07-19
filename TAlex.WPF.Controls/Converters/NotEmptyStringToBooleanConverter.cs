using System;
using System.Globalization;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts not empty <see cref="System.String"/> values to boolean values.
    /// </summary>
    [ValueConversion(typeof(String), typeof(Boolean))]
    public class NotEmptyStringToBooleanConverter : ConverterBase<NotEmptyStringToBooleanConverter>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates that the result of converting must be take the inverse value.
        /// </summary>
        public bool IsReversed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts not empty string to boolean value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>true if <paramref name="value"/> is not null or empty string; otherwise, false.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string s = value.ToString();
                return IsReversed ? IsNullOrWhiteSpace(s) : !IsNullOrWhiteSpace(s);
            }

            return IsReversed;
        }

        private static bool IsNullOrWhiteSpace(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return true;
            }
            else
            {
                foreach (char ch in s)
                {
                    if (!Char.IsWhiteSpace(ch))
                        return false;
                }

                return true;
            }
        }

        #endregion
    }
}
