using Blazored.LocalStorage;

namespace PersonalSite.Theming;

public static class ServicesExtensions
{
    public static IServiceCollection AddThemeServices(this IServiceCollection services)
    {
        services.AddScoped<ISystemColorSchemeDetector, SystemColorSchemeDetector>();

        if(Environment.GetEnvironmentVariable("PRERENDER") != null) 
        {
            services.AddScoped<IThemeManager>(sp =>
            {
                var tm = new MockThemeManager();
                return tm;
            });

            return services;
        }

        services.AddScoped<IThemeManager>(sp =>
        {
            var tm = new ThemeManager(sp.GetRequiredService<ISystemColorSchemeDetector>(), sp.GetRequiredService<ISyncLocalStorageService>());
            tm.Initialize();
            return tm;
        });

        return services;
    }
}