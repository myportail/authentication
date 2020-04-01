using Authlib.Attributes;

namespace authService.Configuration
{
    public class AuthdbSettings
    {
        [LoggableSettings]
        public AuthDbConnectionSettings Connection { get; set; }
    }
}
