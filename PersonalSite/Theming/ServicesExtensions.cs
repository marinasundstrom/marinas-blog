using Blazored.LocalStorage;

namespace PersonalSite.Theming;

public static class ServicesExtensions
{
    public static IServiceCollection AddThemeServices(this IServiceCollection services)
    {
        services.AddScoped<ISystemColorSchemeDetector, SystemColorSchemeDetector>();

        services.AddScoped<IThemeManager>(sp =>
        {
            var tm = new ThemeManager(sp.GetRequiredService<ISystemColorSchemeDetector>(), sp.GetRequiredService<ISyncLocalStorageService>());
            tm.Initialize();
            return tm;
        });

        /*
        services.AddScoped<IThemeManager>(sp =>
        {
            var tm = new MockThemeManager();
            return tm;
        });*/

        return services;
    }
}