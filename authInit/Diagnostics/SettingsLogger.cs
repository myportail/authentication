using System.Collections.Generic;
using System.Reflection;
using Authlib.Attributes;
using Microsoft.Extensions.Logging;

namespace authInit.Diagnostics
{
    public class SettingsLogger
    {
        private ILogger Logger { get; }
        private List<string> Path { get; }

        private string DisplayPath
        {
            get
            {
                return string.Join("", Path);
            }
        }

        public static void LogSettings(string settingRoot, object initialSetting, ILogger logger)
        {
            var settingsLogger = new SettingsLogger(logger, settingRoot);
            settingsLogger.Log(initialSetting);
        }

        private SettingsLogger(
            ILogger logger,
            string initialPath)
        {
            Logger = logger;
            Path = new List<string>()
            {
                initialPath
            };
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
                if (prop.PropertyType.IsGenericType && (prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    int idx = 0;
                    var list = prop.GetValue(setting) as IEnumerable<object>;
                    if (list != null)
                    {
                      foreach (var item in list)
                      {
                        LogSubItem(idx, item);
                        idx++;
                      }
                    }
                }
                else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    LogSubItem(prop.Name, prop.GetValue(setting));
                }
                else if (prop.PropertyType == typeof(string))
                {
                    Logger.LogInformation($"{DisplayPath}.{prop.Name} --> {GetPropValue(prop, setting, attribute)}");
                }
            }
        }

        private string GetPropValue(PropertyInfo prop, object setting, LoggableSettings attribute)
        {
          var value = prop.GetValue(setting)?.ToString();

          if (attribute.Output == LoggableSettingOutput.Secured && value != null)
          {
              return new string('*', value.Length);
          }

          return value;
        }

        private void LogSubItem(int index, object setting)
        {
            Path.Add($"[{index}]");
            Log(setting);
            Path.RemoveAt(Path.Count - 1);
        }

        private void LogSubItem(string name, object setting)
        {
            Path.Add($".{name}");
            Log(setting);
            Path.RemoveAt(Path.Count - 1);
        }
    }
}
