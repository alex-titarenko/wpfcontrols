using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    [ValueConversion(typeof(Int32), typeof(double))]
    public class Int32ToDoubleConverter : ConverterBase<Int32ToDoubleConverter>
    {
        /// <summary>
        /// Converts a 32-bit signed integer number to a double number.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>double number that converted from <paramref name="value"/>.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)((int)value);
        }

        /// <summary>
        /// Converts a double number to a 32-bit signed integer number.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>32-bit signed integer number that converted from <paramref name="value"/>.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((double)value);
        }
    }
}
