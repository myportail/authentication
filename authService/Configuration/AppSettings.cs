using Authlib.Attributes;

namespace authService.Configuration
{
    public class AppSettings
    {
        [LoggableSettings()]
        public string Version { get; set; }
    }
}
