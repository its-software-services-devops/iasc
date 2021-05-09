using Serilog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Vaults
{
    public class VaultProviderSecretFile : VaultProviderBase
    {
        private readonly Regex keyValueRegex = new Regex(@"^(.*?)=(.*)$");
        private readonly Regex gsutilRegex = new Regex(@"^gs://");
        private ICommandExecutor cmdExec = new CommandExecutor();
        private string gsutilCmd = "gsutil";

        public VaultProviderSecretFile(VaultConfig cfg) : base(cfg)
        {
            var gsutilPath = Environment.GetEnvironmentVariable("IASC_GSUTIL_PATH");
            if (gsutilPath != null)
            {
                gsutilCmd = gsutilPath;
            }
        }

        private void AddSecretsArray(IEnumerable<string> arr)
        {
            foreach (var lineRead in arr)
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

        private void LoadSecrets(string fname)
        {
            IEnumerable<string> linesRead = null;
            if (gsutilRegex.IsMatch(fname))
            {
                string args = String.Format("cat {0}", fname);
                string content = cmdExec.Exec(gsutilCmd, args);

                char[] delims = new[] { '\r', '\n' };
                linesRead = content.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                linesRead = File.ReadLines(fname);
            }

            AddSecretsArray(linesRead);
        }

        public void SetCmdExecutor(ICommandExecutor cmd)
        {
            cmdExec = cmd;
        }

        public void SetGsutilCmd(string cmd)
        {
            gsutilCmd = cmd;
        }

        public override void Load()
        {
            if (config.SecretFileName == null)
            {
                Log.Information("Secret file name [{0}] is null, so no secrets loaded!!!", config.SecretFileName);
                return;
            }

            //Do loading secret here
            LoadSecrets(config.SecretFileName);
        }
    } 
}

