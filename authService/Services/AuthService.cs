using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authlib.Configuration;
using Authlib.Services;
using authService.Exceptions;
using authService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace authService.Services
{
    public class AuthService : IAuthService
    {
        private IPasswordHasher PasswordHasher { get; }
        
        private TokenGenerationSettings TokenGenerationSettings { get; }
        
        private IUsersService UsersService { get; }

        public AuthService(
            IOptions<TokenGenerationSettings> tokenGenerationSettings,
            IUsersService usersService,
            IPasswordHasher passwordHasher )
        {
            PasswordHasher = passwordHasher;
            TokenGenerationSettings = tokenGenerationSettings.Value;
            UsersService = usersService;
        }

        public async Task<JwtSecurityToken> CreateToken(Model.Api.LoginCredentials credentials)
        {
            try
            {
                List<Model.MongoDb.User> users = null;
                
                var user = await UsersService.GetUserByName(credentials.Username);

                var hashedPwd = PasswordHasher.HashPassword(credentials.Password);
                if (!user.Password.Equals(hashedPwd))
                    throw new Exception("invalid credentials");
                
                var claims = new[]
                {
                    new Claim("id", user.Id),
                    new Claim("username", user.Name)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenGenerationSettings.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: TokenGenerationSettings.Issuer,
                    audience: TokenGenerationSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
