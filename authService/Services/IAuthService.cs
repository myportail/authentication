

using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using authService.Models.Api;

namespace authService.Services
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateToken(LoginCredentials credentials);
    }
}
