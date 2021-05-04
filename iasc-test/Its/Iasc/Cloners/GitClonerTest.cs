using NUnit.Framework;
using Its.Iasc.Workflows;

namespace Its.Iasc.Cloners
{
    public class GitClonerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("srcdir", "folder")]
        [TestCase("srcdir", "")]
        public void CloneTest(string srcDir, string folder)
        {
            //To make code coverate 100%

            var ctx = new Context();
            ctx.SourceDir = srcDir;
            ctx.VcsFolder = folder;

            var cn = new GitCloner();
            cn.SetCopyCmd("echo");
            cn.SetGitCmd("echo");
            cn.SetContext(ctx);

            cn.Clone();
        }
    }
}