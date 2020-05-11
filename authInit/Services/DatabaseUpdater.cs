using System;
using System.Threading.Tasks;
using authInit.Contexts;
using Authlib.Configuration;
using Authlib.Models.Db;
using Authlib.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace authInit.Services
{
    public class DatabaseUpdater : IDatabaseUpdater
    {
        private ILogger Logger { get; }
        private IServiceProvider ServiceProvider { get; }
        private IPasswordHasher PasswordHasher { get; } 
        private DefaultUserSettings DefaultUserSettings { get; }

        public DatabaseUpdater(
            ILogger<DatabaseUpdater> logger,
            IServiceProvider serviceProvider,
            IPasswordHasher passwordHasher,
            IOptions<DefaultUserSettings> defaultUserSettings )
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            PasswordHasher = passwordHasher;
            DefaultUserSettings = defaultUserSettings.Value;

            Logger.LogInformation("DatabaseUpdater creation");
        }

        public async Task UpdateDb()
        {
            var updateSucceeded = false;
            do
            {
                Logger.LogInformation("Attemping to update database");
                updateSucceeded = await TryUpdateDb();
                if (!updateSucceeded)
                {
                    Logger.LogInformation("database update failed : waiting 30 sec to retry");
                    await Task.Delay(30 * 1000);
                }
            } while (!updateSucceeded);
            
            Logger.LogInformation("database update successful");
        }

        private async Task<Boolean> TryUpdateDb()
        {
            try
            {
                using var scope = ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetService<UserContext>();
                await context.Database.MigrateAsync();
                
                await CreateDefaultAdminUser(context);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private async Task CreateDefaultAdminUser(UserContext  userContext)
        {
            var existingAdminUser = await userContext.Users.FirstOrDefaultAsync(x => x.Name.Equals("Admin", StringComparison.InvariantCultureIgnoreCase));
            if (existingAdminUser == null && !String.IsNullOrEmpty(DefaultUserSettings.Username) && !String.IsNullOrEmpty(DefaultUserSettings.Password))
            {
                var user = new User();
                user.Id = Guid.NewGuid().ToString();
                user.Name = DefaultUserSettings.Username;
                user.Password = PasswordHasher.HashPassword(DefaultUserSettings.Password);
                await userContext.Users.AddAsync(user);
                await userContext.SaveChangesAsync();
            }
        }
    }
}
