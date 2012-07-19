using System;
using System.Globalization;
using System.Windows.Data;


namespace TAlex.WPF.Converters
{
    /// <summary>
    /// Represents the converter that converts not null values to boolean values.
    /// </summary>
    [ValueConversion(typeof(Object), typeof(Boolean))]
    public class IsNotNullToBooleanConverter : ConverterBase<IsNotNullToBooleanConverter>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates that the result of converting must be take the inverse value.
        /// </summary>
        public bool IsReversed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts not null value to boolean value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">The converter parameter to use. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>true if <paramref name="value"/> is not null; otherwise, false.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IsReversed ? (value == null) : (value != null);
        }

        #endregion
    }
}
