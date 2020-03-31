using Authlib.Attributes;

namespace authInit.Configuration
{
    public class AuthdbSettings
    {
        [LoggableSettings]
        public AuthDbConnectionSettings Connection { get; set; }
    }
}
