using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TAlex.WPF.Converters
{
    [ValueConversion(typeof(String), typeof(int))]
    internal class WidthUnitToMaxWidthConverter : ConverterBase<WidthUnitToMaxWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string unit = (string)value;
                switch (unit)
                {
                    case "%": return 100;
                    case "px": return 999999;
                }
            }

            return null;
        }
    }
}
