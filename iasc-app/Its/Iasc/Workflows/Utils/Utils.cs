using Serilog;
using System.Diagnostics;

namespace Its.Iasc.Workflows.Utils
{
    public static class Utils
    {
        public static string Exec(string cmd, string argv)
        {
            string output = "";
            string cmdWithArg = string.Format("{0} {1}", cmd, argv);
            
            Log.Information("Executing command [{0}]...", cmdWithArg);

            using(Process pProcess = new Process())
            {
                pProcess.StartInfo.FileName = cmd;
                pProcess.StartInfo.Arguments = argv;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                pProcess.Start();
                output = pProcess.StandardOutput.ReadToEnd();
                pProcess.WaitForExit();
            }

            Log.Debug(output);
            return output;            
        } 
    }
}
