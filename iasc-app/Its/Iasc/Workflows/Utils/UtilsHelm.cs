using System.Diagnostics;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows.Utils
{
    public static class UtilsHelm
    {
        private static string helmCmd = "helm";
        private static string helmAddArg = "repo add {0} {1}";

        public static void HelmAdd(Infra cfg)
        {
            string arg = string.Format(helmAddArg, cfg.Alias, cfg.ChartUrl);
            using(Process pProcess = new Process())
            {
                pProcess.StartInfo.FileName = helmCmd;
                pProcess.StartInfo.Arguments = arg;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                pProcess.Start();
                string output = pProcess.StandardOutput.ReadToEnd();
                pProcess.WaitForExit();
            }
        }

        public static void HelmTemplate(Infra cfg)
        {
        }        
    }
}
