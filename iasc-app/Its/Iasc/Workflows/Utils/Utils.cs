using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using Its.Iasc.Workflows.Models;
using Serilog;
namespace Its.Iasc.Workflows.Utils
{
    public static class Utils
    {
        private static Manifest manifest = null;

        public static void SetManifest(Manifest mnf)
        {
            manifest = mnf;
        }

        private static string ReplaceString(string argv, Dictionary<string, string> maps)
        {
            var tmp = argv;
            foreach (string variable in maps.Keys) 
            {
                tmp = tmp.Replace(variable, maps[variable]);
            }

            return tmp;
        }

        public static (string execStr, string dispStr) GetInterpolateStrings(string argv, string keyword)
        {
            Dictionary<string, string> maps = new Dictionary<string, string>();
            Dictionary<string, string> displayMap = new Dictionary<string, string>();

            string varPattern = @"(\$\{(" + keyword + @")\..+?\})";
            MatchCollection matches = Regex.Matches(argv, varPattern);

            Regex subPatternDefault = new Regex(@"^\$\{(" + keyword + @")\.(.+)\:(.+)\}$");
            Regex subPattern = new Regex(@"^\$\{(" + keyword + @")\.(.+)\}$"); 

            string bucket = "";
            string key = "";
            string defaultValue = "";
            string actualValue = "";
            string displayValue = "";

            foreach (Match match in matches)
            {
                string variable = match.Groups[1].Value;
                if (maps.ContainsKey(variable))
                {
                    continue;
                }

                if (subPatternDefault.IsMatch(variable))
                {
                    var m = subPatternDefault.Match(variable);

                    bucket = m.Groups[1].Value;
                    key = m.Groups[2].Value;
                    defaultValue = m.Groups[3].Value;
                }
                else if (subPattern.IsMatch(variable))
                {
                    var m = subPattern.Match(variable);
                    
                    bucket = m.Groups[1].Value;
                    key = m.Groups[2].Value;                 
                }

                if (bucket.Equals("VAR"))
                {
                    if (manifest.Vars.ContainsKey(key))
                    {
                        actualValue = manifest.Vars[key];
                    }
                    else
                    {
                        Log.Warning("Variable [{0}] not found, so use default value [{1}] instead", variable, defaultValue);
                        actualValue = defaultValue;
                    }

                    displayValue = actualValue;
                }
                else if (bucket.Equals("ENV"))
                {
                    actualValue = Environment.GetEnvironmentVariable(key);
                    if (actualValue == null)
                    {
                        Log.Warning("Env variable [{0}] not found, so use default value [{1}] instead", variable, defaultValue);
                        actualValue = defaultValue;
                    }

                    displayValue = actualValue;
                }
                else
                {
                    //Secret things
                    displayValue = "***";
                }

                maps[variable] = actualValue;
                displayMap[variable] = displayValue;
            }

            string execStr = ReplaceString(argv, maps);
            string displayStr = ReplaceString(argv, displayMap);

            return (execStr, displayStr);
        }

        public static string Exec(string cmd, string argv)
        {
            string output = "";

            (string execStr, string dispStr) = GetInterpolateStrings(argv, "VAR|ENV");
            string cmdWithArg = string.Format("{0} {1}", cmd, dispStr);
            
            Log.Information("Executing command [{0}]...", cmdWithArg);

            using(Process pProcess = new Process())
            {
                pProcess.StartInfo.FileName = cmd;
                pProcess.StartInfo.Arguments = execStr;
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
