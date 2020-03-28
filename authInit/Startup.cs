using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using authInit.Contexts;
using authInit.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace authInit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Configuration.Application AppConfiguration
        {
            get { return this.Configuration.GetSection("App").Get<Configuration.Application>(); }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(AppConfiguration.AuthDb.Connection.ConnectionString);
            });

            services.AddSingleton<IDatabaseUpdater, DatabaseUpdater>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Using DB Connection : server = {AppConfiguration.AuthDb.Connection.Server} / db = {AppConfiguration.AuthDb.Connection.Database} / user = {AppConfiguration.AuthDb.Connection.User}");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            UpdateDb(app).ContinueWith((Task old) => { Environment.Exit(1); });
        }
        
        async Task<Boolean> UpdateDb(IApplicationBuilder app)
        {
            try
            {
                using var serviceScope = app.ApplicationServices.CreateScope();
                var databaseUpdater = serviceScope.ServiceProvider.GetService<IDatabaseUpdater>();

                await databaseUpdater.UpdateDb();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        void GetSecrets()
        {
            var macos = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            var startInfo = new ProcessStartInfo
            {
                FileName = "/usr/local/bin/kubectl",
                Arguments = "get secret authdbsecrets --namespace=myportail -o jsonpath=\"{.data.mysqlusername}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            System.Console.WriteLine(startInfo.Arguments);

            var process = new Process()
            {
                StartInfo = startInfo
            };

            process.Start();
            string encodedResult = process.StandardOutput.ReadToEnd();
            string result = Encoding.Default.GetString(Convert.FromBase64String(encodedResult));

            System.Console.WriteLine(result);

            process.WaitForExit();
        }
    }
}
