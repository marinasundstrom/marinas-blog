namespace PersonalSite.Theming;

public interface ISystemColorSchemeDetector : IDisposable
{
    ColorScheme CurrentColorScheme { get; }

    event EventHandler<SystemColorSchemeChangedEventArgs> ColorSchemeChanged;
}
