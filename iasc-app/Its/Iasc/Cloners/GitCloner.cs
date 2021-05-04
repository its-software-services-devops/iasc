using System;
using System.IO;
using System.Text;  
using System.Security.Cryptography;  
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Cloners
{
    public class GitCloner : BaseCloner
    {
        private string gitCmd = "git";
        private string copyCmd = "cp";
        private string cloneArgs = "clone -b {0} --single-branch {1} {2}";

        public GitCloner()
        {
        }

        private string GetHash(string inp)
        {
            var builder = new StringBuilder();  
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inp));  
  
                // Convert byte array to a string   
                builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }      
            }

            return builder.ToString();
        }

        public override void Clone()
        {
            string destDir = context.SourceDir;

            bool inTemp = false;
            if (!String.IsNullOrEmpty(context.VcsFolder))
            {
                string hash = GetHash(DateTime.Now.ToString());

                destDir = String.Format("{0}/{1}", Path.GetTempPath(), hash);
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