using Authlib.Attributes;

namespace Authlib.Configuration
{
    public class TokenGenerationSettings
    {
        [LoggableSettings()]
        public string SecurityKey { get; set; }
    }
}
