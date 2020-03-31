using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace authInit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables().Build(); })
                .ConfigureAppConfiguration((hostingContext, config) => { AddKubeCtlConfiguration(hostingContext, config); })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static void AddKubeCtlConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            var kubeCtlSettings = configuration.GetSection("KubeCtl").Get<Configuration.KubeCtlSettings>();
            if (kubeCtlSettings != null)
            {
                builder.Add(new KubeCtl.SecretsSource(kubeCtlSettings, "myportail", "authdbsecrets"));
            }
        }
    }
}
