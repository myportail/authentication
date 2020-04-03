using System;
using System.Threading.Tasks;
using authInit.Contexts;
using authInit.Services;
using Authlib.Configuration;
using Authlib.Diagnostics;
using Authlib.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace authInit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Configuration.AuthdbSettings AuthdbSettings => this.Configuration.GetSection("authdb").Get<Configuration.AuthdbSettings>();

        private string Version => this.Configuration.GetSection("Version").Get<string>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuration.AuthdbSettings>(Configuration.GetSection("authdb"));
            services.Configure<Configuration.KubeCtlSettings>(Configuration.GetSection("KubeCtl"));
            services.Configure<TokenGenerationSettings>(Configuration.GetSection("TokenGeneration"));
            services.Configure<DefaultUserSettings>(Configuration.GetSection("DefaultUser"));

            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(AuthdbSettings.Connection.ConnectionString);
            });

            services.AddSingleton<IDatabaseUpdater, DatabaseUpdater>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Version : {Version}");
            LogSettings(app, logger);

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

        private void LogSettings(IApplicationBuilder app, ILogger logger)
        {
            SettingsLogger.LogSettings(
                "authdb", 
                app.ApplicationServices.GetService<IOptions<Configuration.AuthdbSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "KubeCtl",
                app.ApplicationServices.GetService<IOptions<Configuration.KubeCtlSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "TokenGeneration",
                app.ApplicationServices.GetService<IOptions<TokenGenerationSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "DefaultUser",
                app.ApplicationServices.GetService<IOptions<DefaultUserSettings>>().Value, 
                logger);
        }
    }
}
