using Authlib.Attributes;

namespace Authlib.Configuration
{
    public class TokenGenerationSettings
    {
        [LoggableSettings()]
        public string SecurityKey { get; set; }
        [LoggableSettings]
        public string Issuer { get; set; }
        [LoggableSettings]
        public string Audience { get; set; }
    }
}
