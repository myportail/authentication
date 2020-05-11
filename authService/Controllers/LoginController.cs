using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Authlib.Services;
using authService.Models.Api.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace authService.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private Services.IAuthService AuthService { get; }
        private IPasswordHasher PasswordHasher { get; }

        public LoginController(
            Services.IAuthService authService,
            IPasswordHasher passwordHasher)
        {
            AuthService = authService;
            PasswordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            try
            {
                var hashedPwd = PasswordHasher.HashPassword(credentials.Password);
                var token = await AuthService.CreateToken(credentials.Username, credentials.Password);
                
                // await MongoDbService.Init();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }
    }
}
