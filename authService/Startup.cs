﻿using System;
using System.Text;
using Authlib.Configuration;
using Authlib.Diagnostics;
using Authlib.Services;
using authService.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
            services.Configure<Configuration.AuthdbSettings>(Configuration.GetSection("authdb"));
            services.Configure<TokenGenerationSettings>(Configuration.GetSection("TokenGeneration"));

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                
            });
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            try
            {
                LogSettings(app, logger);

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });

                app.UseAuthentication();
                
                app.UseDefaultFiles();
                app.UseStaticFiles();

                app.UseHsts();
                app.UseRouting();
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
                "authdb", 
                app.ApplicationServices.GetService<IOptions<Configuration.AuthdbSettings>>().Value, 
                logger);
        }
    }
}
