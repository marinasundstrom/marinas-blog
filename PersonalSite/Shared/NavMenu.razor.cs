using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace PersonalSite.Shared;

[SupportedOSPlatform("browser")]
public partial class NavMenu
{
    [JSImport("init", "NavMenu")]
    internal static partial string InitNavbar();

    [JSImport("initCM", "NavMenu")]
    internal static partial string InitColorModes();

    [JSImport("scrollableNavbar", "NavMenu")]
    internal static partial string ScrollableNavbar();
}