using System.Collections.Generic;

namespace Its.Iasc.Vaults
{
    public abstract class VaultProviderBase : IVaultProvider
    {
        protected readonly VaultConfig config = null;
        private Dictionary<string, string> secretMap = new Dictionary<string, string>();

        public abstract void Load();

        protected void AddKeyValue(string key, string value)
        {
            secretMap[key] = value;
        }

        public VaultProviderBase(VaultConfig cfg)
        {
            config = cfg;
        }

        public string GetValue(string key)
        {
            return secretMap.GetValueOrDefault<string, string>(key, null);
        }
    } 
}

