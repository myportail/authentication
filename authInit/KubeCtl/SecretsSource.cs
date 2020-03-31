using System;
using Microsoft.Extensions.Configuration;

namespace authInit.KubeCtl
{
    public class SecretsSource : IConfigurationSource
    {
        private Configuration.KubeCtlSettings KubeCtlSettings { get; }
        private string VaultNamespace { get; }
        private string SecretsVaultName { get; }

        public SecretsSource(
            Configuration.KubeCtlSettings kubeCtlSettings,
            string vaultNamespace,
            string secretsVaultName)
        {
            KubeCtlSettings = kubeCtlSettings;
            VaultNamespace = vaultNamespace;
            SecretsVaultName = secretsVaultName;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecretsProvider(KubeCtlSettings, VaultNamespace, SecretsVaultName);
        }
    }
}
