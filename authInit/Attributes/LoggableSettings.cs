using System;

namespace authInit.Attributes
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
