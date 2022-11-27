namespace PersonalSite.Theming;

public sealed class MockSystemColorSchemeDetector : ISystemColorSchemeDetector
{
    public ColorScheme CurrentColorScheme => ColorScheme.Light;

    public event EventHandler<SystemColorSchemeChangedEventArgs> ColorSchemeChanged = default!;

    public void Dispose()
    {
        
    }
}


public sealed class MockThemeManager : IThemeManager
{
    public ColorScheme CurrentColorScheme => ColorScheme.Light;

    public ColorScheme? PreferredColorScheme { get => ColorScheme.Light; set { } }

    public event EventHandler<ColorSchemeChangedEventArgs> ColorSchemeChanged = default!;

    public void Dispose()
    {

    }

    public void Initialize()
    {

    }

    public void SetPreferredColorScheme(ColorScheme colorScheme)
    {

    }

    public void UseSystemScheme()
    {
        
    }
}