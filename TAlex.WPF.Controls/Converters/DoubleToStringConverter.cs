using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts a double-precision floating-point numbers to and from string values.
    /// </summary>
    [ValueConversion(typeof(Double), typeof(String))]
    public class DoubleToStringConverter : ConverterBase<DoubleToStringConverter>
    {
        /// <summary>
        /// Converts a double-precision floating-point number to equivalent string representation.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns><see cref="System.String"/> that represented a double-precision floating-point number <paramref name="value"/>.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number = (double)value;
            return number.ToString(culture);
        }

        /// <summary>
        /// Converts a string value to a double-precision floating-point number.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>double-precision floating-point number that converted from <paramref name="value"/>.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as String;
            double number;

            if (Double.TryParse(str, NumberStyles.Any, culture, out number))
            {
                return number;
            }

            return new ValidationResult(false, String.Format("Input string '{0}' was not in a correct format.", str));
        }
    }
}
