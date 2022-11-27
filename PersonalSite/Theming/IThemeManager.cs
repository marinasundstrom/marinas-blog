namespace PersonalSite.Theming;

public interface IThemeManager : IDisposable
{
    ColorScheme CurrentColorScheme { get; }
    ColorScheme? PreferredColorScheme { get; set; }

    event EventHandler<ColorSchemeChangedEventArgs> ColorSchemeChanged;

    void Initialize();
    void SetPreferredColorScheme(ColorScheme colorScheme);
    void UseSystemScheme();
}
