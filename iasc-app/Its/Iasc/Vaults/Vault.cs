using System.Collections.Generic;

namespace Its.Iasc.Vaults
{
    public static class Vault
    {
        private static List<IVaultProvider> providers = new List<IVaultProvider>();

        public static void Setup(string cfgFile)
        {
            //In the future we may load secret from many vault providers
            //But support only one for now

            var secretCfg = new VaultConfig() { SecretFileName = cfgFile };
            var secretFile = new VaultProviderSecretFile(secretCfg);

            providers.Add(secretFile);
        }

        public static void Load()
        {
            foreach (IVaultProvider provider in providers)
            {
                provider.Load();
            }
        }

        public static string GetValue(string key)
        {
            foreach (IVaultProvider provider in providers)
            {
                var result = provider.GetValue(key);
                if (result != null)
                {
                    return result;
                }
            }

            //Not found
            return null;
        }
    } 
}

