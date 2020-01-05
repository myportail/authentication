using System;
using System.Threading;
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
                options.UseMySql(AppConfiguration.AuthDb.ConnectionString);
            });

            services.AddSingleton<IDatabaseUpdater, DatabaseUpdater>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            UpdateDb(app).Wait();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        async Task<Boolean> UpdateDb(IApplicationBuilder app)
        {
            try
            {
                using var serviceScope = app.ApplicationServices.CreateScope();
                var databaseUpdater = serviceScope.ServiceProvider.GetService<IDatabaseUpdater>();

                databaseUpdater.UpdateDb();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
