using authInit.Attributes;

namespace Configuration
{
    public class AuthdbSettings
    {
        [LoggableSettings]
        public AuthDbConnectionSettings Connection { get; set; }
    }
}
