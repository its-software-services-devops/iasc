using Serilog;
using System.IO;

namespace Its.Iasc.Vaults
{
    public class VaultProviderSecretFile : VaultProviderBase
    {
        public VaultProviderSecretFile(VaultConfig cfg) : base(cfg)
        {
        }

        private void LoadSecrets(string fname)
        {

        }

        public override void Load()
        {
            if (config.SecretFileName == null)
            {
                Log.Information("Secret file name [{0}] is null, so no secrets loaded!!!", config.SecretFileName);
                return;
            }

            bool fileExist = File.Exists(config.SecretFileName);
            if (!fileExist)
            {
                Log.Information("Secret file name [{0}] not found, so no secrets loaded!!!", config.SecretFileName);
                return;
            }

            //Do loading secret here
            LoadSecrets(config.SecretFileName);
        }
    } 
}

