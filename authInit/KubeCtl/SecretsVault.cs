using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace authInit.KubeCtl
{
    public class SecretsVault
    {
        private Configuration.KubeCtlSettings KubeCtlSettings { get; }
        private Configuration.KubeCtlApplicationSettings KubeCtlAppSettings { get; }
        private string OS { get; }
        public SecretsVault(Configuration.KubeCtlSettings kubeCtlSettings, ILogger logger)
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
                logger.LogError("unable to find current platform");
            }

            KubeCtlAppSettings = kubeCtlSettings.Applications.Find(x => x.OS == OS);
            if (KubeCtlAppSettings == null)
            {
                logger.LogError($"Unable to find kubectl application settings for platform {OS}");
            }
        }

        public void Load(string vaultNamespace, string secretsVaultName)
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

            System.Console.WriteLine(jsonResult);

            process.WaitForExit();
        }
    }
}
