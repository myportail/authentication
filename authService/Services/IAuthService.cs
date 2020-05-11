

using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using authService.Models.Api;
using authService.Models.Api.Parameters;

namespace authService.Services
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateToken(string username, string password);
    }
}
