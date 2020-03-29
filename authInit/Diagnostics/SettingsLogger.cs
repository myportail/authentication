using System.Reflection;
using Microsoft.Extensions.Logging;

namespace authInit.Diagnostics
{
    public class SettingsLogger
    {
        private static int indentSize = 2;
        public static void LogSettings(string path, object settings, ILogger logger)
        {
            LogSettings(path, 0, settings, logger);
        }

        private static void LogSettings(string path, int indent, object settings, ILogger logger)
        {
            var props = settings.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.MemberType == MemberTypes.Property)
                {
                    LogPropertyInfo(path, indent, settings, prop, logger);
                }
                System.Console.WriteLine(prop.Name);
            }
        }

        private static void LogPropertyInfo(string path, int indent, object settings, PropertyInfo prop, ILogger logger)
        {
            var indentStr = new string(' ', indent*indentSize);
            if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
            {
                LogSettings($"{path}.{prop.Name}", indent+1, prop.GetValue(settings), logger);
                return;
            }

            if (prop.PropertyType == typeof(string))
            {
                logger.LogInformation($"{indentStr}{path}.{prop.Name} --> {prop.GetValue(settings)}");
            }
        }
    }
}

