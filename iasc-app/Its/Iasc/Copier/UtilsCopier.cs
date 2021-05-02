using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Its.Iasc.Workflows.Utils;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Copier
{
    public class UtilCopier
    {
        private readonly Regex httpRegex = new Regex(@"^http(s*)://");
        private readonly Regex gsutilRegex = new Regex(@"^gs://");

        private Dictionary<CopyType, string> copyCmds;
        private Dictionary<CopyType, string> copyArgs;

        private string wipDir = "";

        public void SetWipDir(string dir)
        {
            wipDir = dir;
        }

        public void SetCopyCmd(Dictionary<CopyType, string> dict)
        {
            copyCmds = dict;            
        }

        public void SetCopyArg(Dictionary<CopyType, string> dict)
        {
            copyArgs = dict;            
        }

        public (CopyType type, string cmd, string args) GetCpCommand(CopyItem ci)
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

        public string GetDestPath(CopyItem ci, CopyType ct)
        {
            string path = "";
            string dir = "";

            if (!String.IsNullOrEmpty(ci.ToDir))
            {
                path = ci.ToDir;
                dir = ci.ToDir;
            }
            else if (!String.IsNullOrEmpty(ci.ToFile))
            {
                path = ci.ToFile;
                dir = Path.GetDirectoryName(path);
            }

            Utils.Exec("mkdir", String.Format("-p {0}/{1}", wipDir, dir));   

            string dstDir = String.Format("{0}/{1}", wipDir, path);
            string dstPath = dstDir;

            if (ct.Equals(CopyType.Http))
            {
                dstPath = String.Format("-o {0}", dstDir);
            }

            return dstPath;
        }
    }
}
