using Authlib.Attributes;

namespace authService.Configuration
{
    public class StaticFilesSettings
    {
        [LoggableSettings]
        public string RequestPath { get; set; }
    }
}
