using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts Boolean values to and from <see cref="System.Windows.Visibility" /> enumeration values.
    /// </summary>
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanToVisibilityConverter : ConverterBase<BooleanToVisibilityConverter>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates whether to hide the element instead of the collapse.
        /// </summary>
        public bool UseHidden { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts a boolean value to a <see cref="System.Windows.Visibility"/> enumeration value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns><see cref="System.Windows.Visibility.Visible" /> if <paramref name="value" /> is true; otherwise, <see cref="System.Windows.Visibility.Collapsed" /> or <see cref="System.Windows.Visibility.Hidden" />.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullableFlag = (bool?)value;
                flag = nullableFlag.HasValue ? nullableFlag.Value : false;
            }

            return flag ? Visibility.Visible : ((UseHidden) ? Visibility.Hidden : Visibility.Collapsed);
        }

        /// <summary>
        /// Converts a <see cref="System.Windows.Visibility" /> enumeration value to a boolean value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>true if <paramref name="value" /> is <see cref="System.Windows.Visibility.Visible" />; otherwise, false.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Visibility visibility = (Visibility)value;
                return visibility == Visibility.Visible;
            }

            return null;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BooleanToVisibilityConverter() { UseHidden = UseHidden };
        }

        #endregion
    }
}
