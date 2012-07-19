using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts texts in camel notation to texts in regular notation.
    /// </summary>
    [ValueConversion(typeof(String), typeof(String))]
    public class CamelTextToRegularTextConverter : ConverterBase<CamelTextToRegularTextConverter>
    {
        /// <summary>
        /// Converts text in camel notation to text in regular notation.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns><see cref="System.String"/> in regular notation.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                String text = ((String)value).Trim();
                return Regex.Replace(text, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", (m) => " " + m.Value);
            }

            return null;
        }
    }
}
