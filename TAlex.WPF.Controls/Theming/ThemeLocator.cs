namespace TAlex.WPF.Theming
{
    public class ThemeLocator
    {
        public IThemeManager Manager { get; private set; }


        public void SetManager(IThemeManager manager)
        {
            Manager = manager;
        }
    }
}
