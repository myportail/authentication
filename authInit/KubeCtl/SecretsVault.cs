using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;

namespace authInit.KubeCtl
{
  public class SecretsVault
    {
        private Configuration.KubeCtlSettings KubeCtlSettings { get; }
        private Configuration.KubeCtlApplicationSettings KubeCtlAppSettings { get; }
        private string OS { get; }
        public SecretsVault(Configuration.KubeCtlSettings kubeCtlSettings)
        {
            KubeCtlSettings = kubeCtlSettings;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                OS = OSPlatform.OSX.ToString();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                OS = OSPlatform.Windows.ToString();
            }

            if (string.IsNullOrEmpty(OS))
            {
                System.Console.WriteLine("unable to find current platform");
            }

            KubeCtlAppSettings = kubeCtlSettings.Applications.Find(x => x.OS.Equals(OS, StringComparison.InvariantCultureIgnoreCase));
            if (KubeCtlAppSettings == null)
            {
                System.Console.WriteLine($"Unable to find kubectl application settings for platform {OS}");
            }
        }

        public void Load(string vaultNamespace, string secretsVaultName, IConfiguration configuration)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = KubeCtlAppSettings.Path,
                    Arguments = $"get secret {secretsVaultName} --namespace={vaultNamespace} -o json",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string jsonResult = process.StandardOutput.ReadToEnd();

            var jsonDoc = JsonDocument.Parse(jsonResult);
            var root = jsonDoc.RootElement;
            var data = root.GetProperty("data");
            var enumerator = data.EnumerateObject();

            while (enumerator.MoveNext())
            {
                var prop = enumerator.Current;
                var name = prop.Name.Replace('.', ':');
                var base64Value = prop.Value.GetString();
                var value = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(base64Value));
                configuration[name] = value;
                System.Console.WriteLine($"found settings {name} = {value}");
            }

            System.Console.WriteLine(jsonResult);

            process.WaitForExit();
        }
    }
}
