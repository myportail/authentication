using Authlib.Attributes;

namespace authInit.Configuration
{
    public class KubeCtlApplicationSettings
    {
        [LoggableSettings]
        public string OS { get; set; }
        [LoggableSettings]
        public string Path { get; set; }
    }
}
