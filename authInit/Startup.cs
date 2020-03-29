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

        public Configuration.AuthdbSettings AuthdbSettings
        {
            get { return this.Configuration.GetSection("authdb").Get<Configuration.AuthdbSettings>(); }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuration.AuthdbSettings>(Configuration.GetSection("authdb"));

            // Configuration["App:authdb:connection:user"] = "test";
            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(AuthdbSettings.Connection.ConnectionString);
            });

            services.AddSingleton<IDatabaseUpdater, DatabaseUpdater>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Using DB Connection : server = {AuthdbSettings.Connection.Server} / db = {AuthdbSettings.Connection.Database} / user = {AuthdbSettings.Connection.User}");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            GetSecrets();

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

            System.Console.WriteLine(OSPlatform.Windows);
            System.Console.WriteLine(OSPlatform.OSX);

            var startInfo = new ProcessStartInfo
            {
                FileName = "C:\\k8s\\kubectl",
                Arguments = "get secret authdbsecrets --namespace=myportail -o json",
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
            string jsonResult = process.StandardOutput.ReadToEnd();

            System.Console.WriteLine(jsonResult);

            process.WaitForExit();
        }
    }
}
