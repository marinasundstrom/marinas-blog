namespace PersonalSite.Theming;

public class SystemColorSchemeChangedEventArgs : EventArgs
{
    public SystemColorSchemeChangedEventArgs(ColorScheme colorScheme)
    {
        ColorScheme = colorScheme;
    }

    public ColorScheme ColorScheme { get; }
}
