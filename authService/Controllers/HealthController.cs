using System;
using System.Linq;
using System.Threading.Tasks;
using authService.Contexts;
using authService.Models.Api.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthController : Controller
    {
        private UserContext UserContext { get; }
        
        public HealthController(UserContext userContext)
        {
            UserContext = userContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> HealthStatus()
        {
            var status = new HealthStatus()
            {
                DatabaseConnection = await CheckDatabaseConnection()
            };

            return Ok(status);
        }

        private async Task<bool> CheckDatabaseConnection()
        {
            try
            {
                await UserContext.Users.FirstAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
