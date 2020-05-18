using System;
using Microsoft.Extensions.Logging;

namespace Authlib.Extensions.Configuration
{
    public interface IConfigurationEndPoint
    {
        void Log(IServiceProvider serviceProvider, ILogger logger);
    }
}