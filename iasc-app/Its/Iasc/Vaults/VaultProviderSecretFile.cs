using Serilog;
using System.IO;
using System.Text.RegularExpressions;

namespace Its.Iasc.Vaults
{
    public class VaultProviderSecretFile : VaultProviderBase
    {
        private readonly Regex keyValueRegex = new Regex(@"^(.*?)=(.*)$");

        public VaultProviderSecretFile(VaultConfig cfg) : base(cfg)
        {
        }

        private void LoadSecrets(string fname)
        {
            var linesRead = File.ReadLines(fname);
            foreach (var lineRead in linesRead)
            {
                if (!keyValueRegex.IsMatch(lineRead))
                {
                    continue;
                }

                var m = keyValueRegex.Match(lineRead);
                var key = m.Groups[1].Value;
                var value = m.Groups[2].Value;
                AddKeyValue(key, value);
            }            
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

