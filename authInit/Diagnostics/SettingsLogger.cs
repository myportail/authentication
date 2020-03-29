using System.Collections.Generic;
using System.Reflection;
using authInit.Attributes;
using Microsoft.Extensions.Logging;

namespace authInit.Diagnostics
{
    public class SettingsLogger
    {
        private ILogger Logger { get; }
        private Stack<string> Path { get; }

        private string DisplayPath
        {
            get
            {
                return string.Join('.', Path);
            }
        }

        public static void LogSettings(string initialPath, object initialSetting, ILogger logger)
        {
            var settingsLogger = new SettingsLogger(logger, initialPath);
            settingsLogger.Log(initialSetting);
        }

        private SettingsLogger(
            ILogger logger,
            string initialPath)
        {
            Logger = logger;
            Path = new Stack<string>();

            Path.Push(initialPath);
        }

        private void Log(object setting)
        {
            var props = setting.GetType().GetProperties();
            foreach (var prop in props)
            {
                LogSettingProperty(setting, prop);
            }
        }

        private void LogSettingProperty(object setting, PropertyInfo prop)
        {
            var attribute = prop.GetCustomAttribute(typeof(LoggableSettings)) as LoggableSettings;

            if (prop.MemberType == MemberTypes.Property && attribute != null)
            {

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    Path.Push(prop.Name);
                    Log(prop.GetValue(setting));
                    Path.Pop();
                }

                if (prop.PropertyType == typeof(string))
                {
                    Logger.LogInformation($"{DisplayPath}.{prop.Name} --> {GetPropValue(prop, setting, attribute)}");
                }
            }
        }

        private string GetPropValue(PropertyInfo prop, object setting, LoggableSettings attribute)
        {
            var value = prop.GetValue(setting).ToString();

            if (attribute.Output == LoggableSettingOutput.Secured)
            {
                return new string('*', value.Length);
            }

            return value;
        }
    }
}
