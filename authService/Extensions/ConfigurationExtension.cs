using Authlib.Configuration;
using Authlib.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace authService.Extensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection SetupConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            AddConfiguration<Configuration.AppSettings>(services, configuration, "App");
            AddConfiguration<Configuration.AuthdbSettings>(services, configuration, "authdb");
            AddConfiguration<TokenGenerationSettings>(services, configuration, "TokenGeneration");
            AddConfiguration<Configuration.SwaggerSettings>(services, configuration, "Swagger");
            AddConfiguration<Configuration.StaticFilesSettings>(services, configuration, "StaticFiles");
            
            return services;
        }

        private static void AddConfiguration<CONFIG_TYPE>(IServiceCollection services, IConfiguration configuration, string key) where CONFIG_TYPE : class, new()
        {
            services.Configure<CONFIG_TYPE>(configuration.GetSection(key));
            services.AddConfiguration<CONFIG_TYPE>("App");
        }
    }
}
