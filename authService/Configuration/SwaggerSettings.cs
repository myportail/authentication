using Authlib.Attributes;

namespace authService.Configuration
{
    public class SwaggerSettings
    {
        [LoggableSettings]
        public string RoutePrefix { get; set; }
    }
}
