namespace TAlex.WPF.Theming
{
    public class ThemeInfo
    {
        public string Name { get; private set; }
        public virtual string DisplayName { get; private set; }
        public string FamilyName { get; private set; }


        public ThemeInfo(string name, string familyName)
        {
            Name = name;
            DisplayName = name;
            FamilyName = familyName;
        }
    }
}
