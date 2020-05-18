using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Authlib.Extensions.Configuration
{
    public static class ConfigurationExtension
    {
        private static List<IConfigurationEndPoint> Configurations = new List<IConfigurationEndPoint>();

        public static IServiceCollection AddConfiguration<CONFIG_TYPE>(this IServiceCollection services, string key) where CONFIG_TYPE : class, new()
        {
            Configurations.Add(new ConfigurationEndPoint<CONFIG_TYPE>(key));
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
