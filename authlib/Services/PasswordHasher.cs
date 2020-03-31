using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace Authlib.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private Configuration.TokenGenerationSettings TokenGenerationSettings { get; }

        public PasswordHasher(IOptions<Configuration.TokenGenerationSettings> tokenGenerationSettings)
        {
            TokenGenerationSettings = tokenGenerationSettings.Value;
        }
        
        public string HashPassword(string password)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(TokenGenerationSettings.SecurityKey),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
