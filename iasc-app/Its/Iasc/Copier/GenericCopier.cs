using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Its.Iasc.Workflows.Models;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Copier
{
    public class GenericCopier : ICopier
    {
        private readonly Regex httpRegex = new Regex(@"^http(s*)://");
        private readonly Regex gsutilRegex = new Regex(@"^gs://");

        private readonly Dictionary<CopyType, string> copyCmds = new Dictionary<CopyType, string>() 
        { 
            { CopyType.Cp, "cp" },
            { CopyType.GsUtilCp, "gsutil" },
            { CopyType.Http, "curl" }
        };

        private readonly Dictionary<CopyType, string> copyArgs = new Dictionary<CopyType, string>() 
        { 
            { CopyType.Cp, "" }, // Jutst "cp"
            { CopyType.GsUtilCp, "cp" }, //gsutil cp
            { CopyType.Http, "-L" } //curl -LO
        };

        private string wipDir = "";
        private string srcDir = "";

        private (CopyType type, string cmd, string args) GetCpCommand(CopyItem ci)
        {
            string args = copyArgs[CopyType.Cp];
            string cpCmd = copyCmds[CopyType.Cp];

            CopyType type = CopyType.Cp;

            if (httpRegex.IsMatch(ci.From))
            {
                cpCmd = copyCmds[CopyType.Http];
                args = copyArgs[CopyType.Http];
                type = CopyType.Http;
            }
            else if (gsutilRegex.IsMatch(ci.From))
            {
                cpCmd = copyCmds[CopyType.GsUtilCp];
                args = copyArgs[CopyType.GsUtilCp];
                type = CopyType.GsUtilCp;
            }

            return (type, cpCmd, args);
        }

        private string GetDestDir(CopyItem ci)
        {
            string path = "";
            string dir = "";

            if (!string.IsNullOrEmpty(ci.ToDir))
            {
                path = ci.ToDir;
                dir = ci.ToDir;
            }
            else if (!string.IsNullOrEmpty(ci.ToFile))
            {
                path = ci.ToFile;
                dir = Path.GetDirectoryName(path);
            }

            Utils.Exec("mkdir", String.Format("-p {0}/{1}", wipDir, dir));   

            return String.Format("{0}/{1}", wipDir, path);
        }

        public void Process(CopyItem[] copyItems)
        {
            foreach (CopyItem ci in copyItems)
            {
                (CopyType ct, string cmd, string args) = GetCpCommand(ci);

                string srcPath = ci.From;
                if (ct.Equals(CopyType.Cp))
                {
                    srcPath = String.Format("{0}/{1}", srcDir, ci.From);
                }

                string dstDir = GetDestDir(ci);
                string dstPath = dstDir;

                if (ct.Equals(CopyType.Http))
                {
                    dstPath = String.Format("-o {0}", dstDir);
                }

                string argv = String.Format("{0} {1} {2}", args, srcPath, dstPath);
                Utils.Exec(cmd, argv);
            }
        }

        public void SetSrcDir(string sDir)
        {
            srcDir = sDir;
        }

        public void SetWipDir(string wpDir)
        {
            wipDir = wpDir;
        }

        public void SetCopyCmd(CopyType cpType, string cmd)
        {
            copyCmds[cpType] = cmd;
        }
    }
}