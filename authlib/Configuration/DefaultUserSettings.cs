using Authlib.Attributes;

namespace Authlib.Configuration
{
    public class DefaultUserSettings
    {
        [LoggableSettings]
        public string Username { get; set; }
        [LoggableSettings(LoggableSettingOutput.Secured)]
        public string Password { get; set; }
    }
}
