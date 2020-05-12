using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Authlib.Services;
using authService.Models.Api.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authService.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private Services.IAuthService AuthService { get; }

        public LoginController(
            Services.IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            try
            {
                var token = await AuthService.CreateToken(credentials.Username, credentials.Password);

                if (token == null)
                {
                    return Unauthorized();
                }
                // await MongoDbService.Init();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
