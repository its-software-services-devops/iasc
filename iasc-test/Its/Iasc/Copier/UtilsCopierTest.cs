using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Copier
{
    public class UtilCopierTest
    {
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

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("https://dummy.test/all-in-one.yaml", CopyType.Http, "-L")]
        [TestCase("gs://dummy.test/all-in-one.yaml", CopyType.GsUtilCp, "cp")]
        [TestCase("dummy/abc.txt", CopyType.Cp, "")]
        public void GetCpCommandTest(string from, CopyType expType, string expArgs)
        {
            var cp = new UtilCopier();
            cp.SetCopyArg(copyArgs);
            cp.SetCopyCmd(copyCmds);
            cp.SetWipDir("");

            var ci = new CopyItem();
            ci.From = from;

            (CopyType type, string cmd, string args) = cp.GetCpCommand(ci);

            Assert.AreEqual(expType, type);
            Assert.AreEqual(expArgs, args);
        }

        [TestCase("all-in-one.yaml", "", CopyType.Cp, "{0}/all-in-one.yaml")]
        [TestCase("", "dirname/subdir", CopyType.Cp, "{0}/dirname/subdir")]
        [TestCase("all-in-one.yaml", "", CopyType.Http, "-o {0}/all-in-one.yaml")]
        [TestCase("", "dirname/subdir", CopyType.Http, "-o {0}/dirname/subdir")]
        [TestCase("all-in-one.yaml", "", CopyType.GsUtilCp, "{0}/all-in-one.yaml")]
        [TestCase("", "dirname/subdir", CopyType.GsUtilCp, "{0}/dirname/subdir")]
        public void GetDestPathTest(string toFile, string toDir, CopyType type, string expValue)
        {
            string tmpPath = Path.GetTempPath();

            var cp = new UtilCopier();
            cp.SetCopyArg(copyArgs);
            cp.SetCopyCmd(copyCmds);
            cp.SetWipDir(tmpPath);

            var ci = new CopyItem();
            ci.ToFile = toFile;
            ci.ToDir = toDir;

            var dstPath = cp.GetDestPath(ci, type);
            var expPath = String.Format(expValue, tmpPath);

            Assert.AreEqual(expPath, dstPath);
        }        
    }
}