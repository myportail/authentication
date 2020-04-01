using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace authInit.KubeCtl
{
    public class SecretsProvider : ConfigurationProvider
    {
        private Configuration.KubeCtlSettings KubeCtlSettings { get; }
        private string VaultNamespace { get; }
        private string SecretsVaultName { get; }
        public SecretsProvider(
            Configuration.KubeCtlSettings kubeCtlSettings,
            string vaultNamespace,
            string secretsVaultName)
        {
            KubeCtlSettings = kubeCtlSettings;
            VaultNamespace = vaultNamespace;
            SecretsVaultName = secretsVaultName;
        }

        public override void Load()
        {
            var kubeCtlAppSettings = GetKubeCtlApplicationSettings();

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = kubeCtlAppSettings.Path,
                    Arguments = $"get secret {SecretsVaultName} --namespace={VaultNamespace} -o json",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string jsonResult = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            System.Console.WriteLine(jsonResult);

            if (!string.IsNullOrEmpty(jsonResult))
            {
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
                    Data[name] = value;
                    System.Console.WriteLine($"found settings {name} = {value}");
                }
            }
        }

        private Configuration.KubeCtlApplicationSettings GetKubeCtlApplicationSettings()
        {
            string os = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                os = OSPlatform.OSX.ToString();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                os = OSPlatform.Windows.ToString();
            }

            if (string.IsNullOrEmpty(os))
            {
                System.Console.WriteLine("unable to find current platform");
            }

            var appSettings = KubeCtlSettings.Applications.Find(x => x.OS.Equals(os, StringComparison.InvariantCultureIgnoreCase));
            if (appSettings == null)
            {
                System.Console.WriteLine($"Unable to find kubectl application settings for platform {os}");
            }

            return appSettings;
        }
    }
}
