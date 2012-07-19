using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts <see cref="System.Windows.Media.Color"/> values to <see cref="System.Windows.Media.Brush"/> values.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToBrushConverter : ConverterBase<ColorToBrushConverter>
    {
        /// <summary>
        /// Converts a <see cref="System.Windows.Media.Color"/> value to a <see cref="System.Windows.Media.Brush"/> value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns><see cref="System.Windows.Media.Brush"/> converted from <paramref name="value"/>.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Color color = (Color)value;
                return new SolidColorBrush(color);
            }

            return null;
        }
    }
}
