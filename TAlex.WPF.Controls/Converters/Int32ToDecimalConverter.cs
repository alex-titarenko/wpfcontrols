using System;
using System.Globalization;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts a 32-bit signed integer numbers to and from decimal numbers.
    /// </summary>
    [ValueConversion(typeof(Int32), typeof(Decimal))]
    public class Int32ToDecimalConverter : ConverterBase<Int32ToDecimalConverter>
    {
        /// <summary>
        /// Converts a 32-bit signed integer number to a decimal number.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>decimal number that converted from <paramref name="value"/>.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (decimal)((int)value);
        }

        /// <summary>
        /// Converts a decimal number to a 32-bit signed integer number.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>32-bit signed integer number that converted from <paramref name="value"/>.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((decimal)value);
        }
    }
}
