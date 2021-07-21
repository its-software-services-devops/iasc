using System.Collections.Generic;

namespace Its.Iasc.Vaults
{
    public static class Vault
    {
        private static List<IVaultProvider> providers = new List<IVaultProvider>();

        public static void Setup(string cfgFile, string extFile1)
        {
            //In the future we may load secret from many vault providers
            //But support only one for now

            var secretCfg = new VaultConfig() { SecretFileName = cfgFile };
            var secretFile = new VaultProviderSecretFile(secretCfg);
            providers.Add(secretFile);

            if (!string.IsNullOrEmpty(extFile1))
            {
                var secretExt1 = new VaultConfig() { SecretFileName = extFile1 };
                var secretFileExt1 = new VaultProviderSecretFile(secretExt1);
                providers.Add(secretFileExt1);
            }
        }

        public static void Load()
        {
            foreach (IVaultProvider provider in providers)
            {
                provider.Load();
            }
        }

        public static void Clear()
        {
            providers.Clear();
        }

        public static string GetValue(string key)
        {
            //Reverse order here
            int cnt = providers.Count;
            for (int i=cnt-1; i>=0; i--)
            {
                var provider = providers[i];

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

