using Microsoft.Extensions.Configuration;

namespace StoreApi.Shared.Settings;

/// <summary>
/// A global configuration provider,
/// you can share the software settings through
/// all the entities of the project, also it can
/// turn the project environments such as 
/// development (DEBUG) and production (RELEASE).
/// The appsettings.json file will be added in all
/// environments.
/// </summary>
public static class ConfigurationProvider
{   
    /// <summary>
    /// Singleton instance of the main configuration provider
    /// </summary>
    /// <value></value>
    private static IConfigurationRoot? Configuration;

    /// <summary>
    /// You can access the shared configurations using this method
    /// </summary>
    public static IConfigurationRoot GetConfiguration(){
        if(Configuration != null) return Configuration;
        
        var env = "Development";
        #if RELEASE
            env = "Production"
        #endif

        var filename = $"shared.appsettings.{env}.json";
        var builder = new ConfigurationBuilder()
        .AddJsonFile("shared.appsettings.json")
        .AddJsonFile(filename);

        Configuration = builder.Build();

        return Configuration;
    }
}
