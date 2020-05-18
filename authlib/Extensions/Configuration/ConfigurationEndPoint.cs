using System;
using Authlib.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authlib.Extensions.Configuration
{
    public class ConfigurationEndPoint<CONFIG_TYPE> : IConfigurationEndPoint where CONFIG_TYPE: class, new()
    {
        private string Key { get; }
        
        public ConfigurationEndPoint(string key)
        {
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
}