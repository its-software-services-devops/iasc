using System;
using System.IO;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Cloners
{
    public class GitCloner : BaseCloner
    {
        private string gitCmd = "git";
        private string copyCmd = "cp";
        private readonly string cloneArgs = "clone -b {0} --single-branch {1} {2}";

        public GitCloner()
        {
        }

        public override void Clone()
        {
            string destDir = context.SourceDir;

            string tmpPath = context.TmpDir;
            if (String.IsNullOrEmpty(tmpPath))
            {
                tmpPath = Path.GetTempPath();
            }

            bool inTemp = false;
            if (!String.IsNullOrEmpty(context.VcsFolder))
            {
                string key = DateTime.Now.ToString("yyyyMMddHHmmss");
                destDir = String.Format("{0}/{1}", tmpPath, key);
                inTemp = true;
            }

            string args = String.Format(cloneArgs, context.VcsRef, context.VcsUrl, destDir);
            Utils.Exec(gitCmd, args);

            if (inTemp)
            {
                string copyArgs = String.Format("-r {0}/{1}/* {2}", destDir, context.VcsFolder, context.SourceDir);
                Utils.Exec(copyCmd, copyArgs);
            }
        }

        public void SetGitCmd(string cmd)
        {
            gitCmd = cmd;
        }

        public void SetCopyCmd(string cmd)
        {
            copyCmd = cmd;
        }        
    }
}