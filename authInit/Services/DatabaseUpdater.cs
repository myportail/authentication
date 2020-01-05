using System;
using System.Threading.Tasks;
using authInit.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace authInit.Services
{
    public class DatabaseUpdater : IDatabaseUpdater
    {
        private ILogger Logger { get; }
        private UserContext UserContext { get; }
        
        public DatabaseUpdater(
            ILogger<DatabaseUpdater> logger,
            UserContext userContext)
        {
            Logger = logger;
            UserContext = userContext;

            Logger.LogDebug("DatabaseUpdater creation");
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
                await UserContext.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

    }
}
