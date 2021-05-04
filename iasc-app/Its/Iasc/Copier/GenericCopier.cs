using System;
using System.Collections.Generic;
using Its.Iasc.Workflows.Models;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Copier
{
    public class GenericCopier : ICopier
    {
        private readonly UtilCopier copier = new UtilCopier();

        private readonly Dictionary<CopyType, string> copyCmds = new Dictionary<CopyType, string>() 
        { 
            { CopyType.Cp, "cp" },
            { CopyType.GsUtilCp, "gstuil" },
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


        public void Process(CopyItem[] copyItems)
        {
            copier.SetWipDir(wipDir);
            copier.SetCopyArg(copyArgs);
            copier.SetCopyCmd(copyCmds);

            foreach (CopyItem ci in copyItems)
            {
                (CopyType ct, string cmd, string args) = copier.GetCpCommand(ci);

                string srcPath = ci.From;
                if (ct.Equals(CopyType.Cp))
                {
                    srcPath = String.Format("{0}/{1}", srcDir, ci.From);
                }

                string dstPath = copier.GetDestPath(ci, ct);

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