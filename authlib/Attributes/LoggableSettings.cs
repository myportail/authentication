using System;

namespace Authlib.Attributes
{
    public class LoggableSettings : Attribute
    {
        public LoggableSettingOutput Output { get; }

        public LoggableSettings(LoggableSettingOutput output = LoggableSettingOutput.Default)
        {
            Output = output;
        }
    }
}
