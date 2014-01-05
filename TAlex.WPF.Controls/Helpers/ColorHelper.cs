using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TAlex.WPF.Helpers
{
    internal static class ColorHelper
    {
        public static string ColorToString(Color color)
        {
            return String.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        public static int BgrToRgb(int color)
        {
            return (color & 0x000000ff) << 16 | (color & 0x0000FF00) | (color & 0x00FF0000) >> 16;
        }
    }
}
