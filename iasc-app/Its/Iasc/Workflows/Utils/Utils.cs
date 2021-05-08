using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using Its.Iasc.Workflows.Models;
using Its.Iasc.Vaults;
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

        private static (string actValue, string dispValue) GetVariableValue(string variable, string bucket, string key, string defaultValue)
        {
            string actualValue = "";
            string displayValue = "";

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
                actualValue = Vault.GetValue(key);
                if (actualValue == null)
                {
                    Log.Warning("Secret variable [{0}] not found", variable);
                    actualValue = defaultValue;
                }

                //Secret things
                displayValue = "***";
            }

            return (actualValue, displayValue);     
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

                (actualValue, displayValue) = GetVariableValue(variable, bucket, key, defaultValue);

                maps[variable] = actualValue;
                displayMap[variable] = displayValue;
            }

            string execStr = ReplaceString(argv, maps);
            string displayStr = ReplaceString(argv, displayMap);

            return (execStr, displayStr);
        }

        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Log.Error("Directory [{0}] not found!!!", sourceDirName);
                return;            
            }

            string name = Path.GetFileName(sourceDirName);
            if (name.StartsWith('.'))
            {
                Log.Information("Skip hidden directory [{0}]", name);
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);

                Log.Information("Copied file [{0}] to [{1}]", file.Name, destDirName);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                CopyDirectory(subdir.FullName, temppath);
            }
        }
        
        public static void CopyFile(string src, string dst)
        {
            (string srcExecStr, string srcDispStr) = GetInterpolateStrings(src, "VAR|ENV");
            (string dstExecStr, string dstDispStr) = GetInterpolateStrings(dst, "VAR|ENV");

            string sourceDir = Path.GetDirectoryName(srcExecStr);
            string fileName = Path.GetFileName(srcExecStr);

            string[] fileList = Directory.GetFiles(sourceDir, fileName);

            bool isDir = Directory.Exists(dstExecStr);
            foreach (string f in fileList)
            {           
                var dstPath = dstExecStr;

                if (isDir)
                {
                    string fname = Path.GetFileName(f);
                    dstPath = Path.Combine(dstExecStr, fname);
                }

                File.Copy(f, dstPath, true);
                Log.Information("Copied file [{0}] to [{1}]", f, dstPath);
            }
        }

        public static string Exec(string cmd, string argv)
        {
            string output = "";

            (string execStr, string dispStr) = GetInterpolateStrings(argv, "VAR|ENV|SEC");
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

                if (pProcess.ExitCode != 0)
                {
                    Environment.Exit(1);
                }
            }

            Log.Debug(output);
            return output;            
        } 
    }
}
