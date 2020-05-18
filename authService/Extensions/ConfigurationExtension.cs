using System;
using System.Collections.Generic;
using Authlib.Configuration;
using Authlib.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace authService.Extensions
{
    interface IConfigurationEndPoint
    {
        void Log(IServiceProvider serviceProvider, ILogger logger);
    }
    
    class ConfigurationEndPoint<CONFIG_TYPE> : IConfigurationEndPoint where CONFIG_TYPE: class, new()
    {
        private string Key { get; }
        
        public ConfigurationEndPoint(IServiceCollection services, IConfiguration configuration, string key)
        {
            services.Configure<CONFIG_TYPE>(configuration.GetSection(key));
            Key = key;
        }
        
        public void Log(IServiceProvider serviceProvider, ILogger logger)
        {
            SettingsLogger.LogSettings(
                "app", 
                GetConfig(serviceProvider), 
                logger);
        }
        
        private CONFIG_TYPE GetConfig(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<IOptions<CONFIG_TYPE>>().Value;
        }
    }
    
    public static class ConfigurationExtension
    {
        private static List<IConfigurationEndPoint> Configurations = new List<IConfigurationEndPoint>();
        
        public static IServiceCollection SetupConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            Configurations.Add(new ConfigurationEndPoint<Configuration.AppSettings>(
                services,
                configuration,
                "App"));
            Configurations.Add(new ConfigurationEndPoint<TokenGenerationSettings>(
                services,
                configuration,
                "authdb"));
            
            services.Configure<TokenGenerationSettings>(configuration.GetSection("TokenGeneration"));
            services.Configure<Configuration.SwaggerSettings>(configuration.GetSection("Swagger"));
            services.Configure<Configuration.StaticFilesSettings>(configuration.GetSection("StaticFiles"));

            return services;
        }

        public static void LogConfigurations(this IServiceProvider services, ILogger logger)
        {
            foreach (var configuration in Configurations)
            {
                configuration.Log(services, logger);
            }
        }
    }
}
