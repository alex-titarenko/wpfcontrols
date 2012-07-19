using System;
using System.Windows.Media;


namespace TAlex.WPF.Media
{
    /// <summary>
    /// Represents the data for color item.
    /// </summary>
    internal class ColorItem
    {
        public Color Color { get; set; }
        public Brush Brush { get { return new SolidColorBrush(Color); } }
        public string Name { get; set; }
        public string ToolTip { get { return Color.ToString(); } }

        public string AdvancedToolTip
        {
            get
            {
                if (Name.StartsWith("#"))
                    return ToolTip;
                else
                    return String.Format("{0}{1}{2}", Name, Environment.NewLine, ToolTip);
            }
        }
    }
}
