using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Authlib.Configuration;
using Authlib.Diagnostics;
using Authlib.Services;
using authService.Contexts;
using authService.Swagger;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace authService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // AppSettings = Configuration.GetSection("App").Get<Settings.Application>();

            // AppSettings.Validate();
        }

        public IConfiguration Configuration { get; }
        // public Settings.Application AppSettings { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.Configure<Configuration.AppSettings>(Configuration.GetSection("App"));
            services.Configure<Configuration.AuthdbSettings>(Configuration.GetSection("authdb"));
            services.Configure<TokenGenerationSettings>(Configuration.GetSection("TokenGeneration"));
            services.Configure<Configuration.SwaggerSettings>(Configuration.GetSection("Swagger"));
            services.Configure<Configuration.StaticFilesSettings>(Configuration.GetSection("StaticFiles"));

            // services.AddMvc();
            services.AddCors();
            
            services.AddDbContext<UserContext>(options =>
            {
                var authdb = Configuration.GetSection("authdb").Get<Configuration.AuthdbSettings>();
                options.UseMySql(authdb.Connection.ConnectionString);
            });
            services.AddScoped<Services.IUsersService, Services.UsersService>();
            services.AddScoped<Services.IAuthService, Services.AuthService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var tokenGeneration = Configuration.GetSection("TokenGeneration").Get<TokenGenerationSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenGeneration.Issuer,
                        ValidAudience = tokenGeneration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(tokenGeneration.SecurityKey))
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "My API",
                    Version = "v1"
                });
                // c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.SchemaFilter<SchemaFilter>();
            });
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddControllers();
            services.AddApiVersioning();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1,0);
                config.ReportApiVersions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            try
            {
                LogSettings(app, logger);

                var swaggerSettings = app.ApplicationServices.GetService<IOptions<Configuration.SwaggerSettings>>()
                    ?.Value;
                var routePrefix = swaggerSettings?.RoutePrefix ?? "";

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSwagger(c => { 
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer()
                            {
                                Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{routePrefix}"
                            }
                        };
                    }); 
                });
                
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{routePrefix}/swagger/v1/swagger.json", "My API V1");
                });
                
                var staticFilesSettings =
                    app.ApplicationServices.GetService <IOptions<Configuration.StaticFilesSettings>>().Value;

                var wwwrootFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(wwwrootFolder))
                {
                    Directory.CreateDirectory(wwwrootFolder);
                }
                
                app.UseDefaultFiles();
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                    RequestPath = new PathString(staticFilesSettings.RequestPath ?? "")
                });

                app.UseHsts();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                throw ex;
            }
        }

        private void LogSettings(IApplicationBuilder app, ILogger logger)
        {
            SettingsLogger.LogSettings(
                "app", 
                app.ApplicationServices.GetService<IOptions<Configuration.AppSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "authdb", 
                app.ApplicationServices.GetService<IOptions<Configuration.AuthdbSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "TokenGeneration",
                app.ApplicationServices.GetService<IOptions<TokenGenerationSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "Swagger",
                app.ApplicationServices.GetService<IOptions<Configuration.SwaggerSettings>>().Value, 
                logger);
            SettingsLogger.LogSettings(
                "StaticFiles",
                app.ApplicationServices.GetService<IOptions<Configuration.StaticFilesSettings>>().Value, 
                logger);
        }
    }
}
