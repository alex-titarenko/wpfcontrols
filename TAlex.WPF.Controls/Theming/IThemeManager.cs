using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace TAlex.WPF.Theming
{
    public interface IThemeManager
    {
        string CurrentTheme { get; }
        ReadOnlyCollection<ThemeInfo> AvailableThemes { get; }

        bool ApplyTheme(string themeName);
        event ThemeChangedEventHandler ThemeChanged;
    }


    public delegate void ThemeChangedEventHandler(object sender, ThemeEventArgs args);


    public class ThemeEventArgs : EventArgs
    {
        public string NewTheme { get; private set; }

        public ThemeEventArgs(string newTheme)
        {
            NewTheme = newTheme;
        }
    }
}
