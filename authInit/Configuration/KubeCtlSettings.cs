using System.Collections.Generic;
using Authlib.Attributes;

namespace authInit.Configuration
{
    public class KubeCtlSettings
    {
        [LoggableSettings]
        public List<KubeCtlApplicationSettings> Applications { get; set; }
    }
}
