using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authlib.Configuration;
using Authlib.Services;
using authService.Models.Api;
using authService.Models.Api.Parameters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<JwtSecurityToken> CreateToken(string username, string password)
        {
            try
            {
                var user = await UsersService.GetUserByName(username);
                if (user == null)
                    return null;

                var hashedPwd = PasswordHasher.HashPassword(password);
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
