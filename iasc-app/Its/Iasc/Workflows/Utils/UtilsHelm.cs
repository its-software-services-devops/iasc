using System.Diagnostics;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows.Utils
{
    public static class UtilsHelm
    {
        private static readonly string defaultHelmCmd = "helm";
        private static string helmCmd = defaultHelmCmd;
        private static string helmAddArg = "repo add {0} {1}";
        private static string helmTplArg = "template {0} {1}/{2} -f {3} --version {4}";

        public static void SetCmd(string cmd)
        {
            helmCmd = cmd;
        }

        public static void ResetHelmCmd()
        {
            helmCmd = defaultHelmCmd;
        }

        public static void HelmAdd(Infra cfg)
        {
            string arg = string.Format(helmAddArg, cfg.Alias, cfg.ChartUrl);
            Utils.Exec(helmCmd, arg);
        }

        public static void HelmTemplate(Infra cfg)
        {
            string arg = string.Format(helmTplArg, cfg.ChartId, cfg.Alias, cfg.ChartId, cfg.ValuesFile, cfg.Version);
            Utils.Exec(helmCmd, arg);
        }        
    }
}
