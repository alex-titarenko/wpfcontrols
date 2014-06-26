namespace TAlex.WPF.Theming
{
    public static class ThemeLocator
    {
        public static IThemeManager Manager { get; private set; }


        public static void SetManager(IThemeManager manager)
        {
            Manager = manager;
        }
    }
}
