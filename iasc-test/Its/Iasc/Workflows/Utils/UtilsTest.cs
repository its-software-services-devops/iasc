using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows
{
    public class UtilsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("https://${VAR.eckFile}/${VAR.eckVersion}", "VAR", "https://abcdef/1.5.1", "https://abcdef/1.5.1")]
        [TestCase("https://${VAR.dummy:notfound}/${VAR.eckVersion}", "VAR", "https://notfound/1.5.1", "https://notfound/1.5.1")]
        [TestCase("https://${VAR.dummy:notfound1}/${VAR.dummy:notfound2}", "VAR", "https://notfound1/notfound2", "https://notfound1/notfound2")]
        [TestCase("https://${VAR.dummy:notfound1}/${VAR.dummy:notfound1}", "VAR", "https://notfound1/notfound1", "https://notfound1/notfound1")]

        [TestCase("https://${ENV.TEST_VAR1}/${VAR.eckVersion}", "VAR|ENV", "https://seubpong/1.5.1", "https://seubpong/1.5.1")]
        [TestCase("https://${ENV.TEST_VAR2:envnotfound}/${VAR.eckVersion}", "VAR|ENV", "https://envnotfound/1.5.1", "https://envnotfound/1.5.1")]
        [TestCase("https://${ENV.TEST_VAR2:envnotfound1}/${ENV.TEST_VAR2:envnotfound2}", "VAR|ENV", "https://envnotfound1/envnotfound2", "https://envnotfound1/envnotfound2")]

        [TestCase("https://${SC.SECRET1:xxxx}/${ENV.TEST_VAR2:envnotfound2}", "SC|VAR|ENV", "https://xxxx/envnotfound2", "https://***/envnotfound2")]
        [TestCase("${SC.SECRET1:xxxx}/${ENV.TEST_VAR2:envnotfound2}", "VAR", "${SC.SECRET1:xxxx}/${ENV.TEST_VAR2:envnotfound2}", "${SC.SECRET1:xxxx}/${ENV.TEST_VAR2:envnotfound2}")]
        public void GetInterpolateStringsTest(string orgString, string bucket, string execValue, string dispValue)
        {
            var manifest = new Manifest();
            manifest.Vars = new Dictionary<string, string>();

            Environment.SetEnvironmentVariable("TEST_VAR1", "seubpong");

            var vars = manifest.Vars;
            vars.Add("eckVersion", "1.5.1");
            vars.Add("eckFile", "abcdef");

            Utils.Utils.SetManifest(manifest);
            (string execStr, string dispStr) = Utils.Utils.GetInterpolateStrings(orgString, bucket);

            Assert.AreEqual(execValue, execStr);
            Assert.AreEqual(dispValue, dispStr);
        } 

        [TestCase("test1/hello.txt", "output1", "hello-new.txt", "output1/hello-new.txt")]
        [TestCase("test1/hello.txt", "output1", "", "output1/hello.txt")]
        [TestCase("test1/hello-${ENV.CopyFileTest}.txt", "output1", "", "output1/hello-seubpong.txt")]
        public void CopyFileTest(string srcPathRaw, string dstDir, string dstFile, string expFilePath)
        {
            Environment.SetEnvironmentVariable("CopyFileTest", "seubpong");
            var (srcPath, notused) = Utils.Utils.GetInterpolateStrings(srcPathRaw, "ENV");

            Manifest mnf = new Manifest();
            Utils.Utils.SetManifest(mnf);

            string tmpDir = Path.GetTempPath();
            string srcDir = Path.GetDirectoryName(srcPath);
            string absTmpSrcDir = String.Format("{0}/{1}", tmpDir, srcDir);
            string absTmpDstDir = String.Format("{0}/{1}", tmpDir, dstDir);

            string absSrcPath = String.Format("{0}/{1}", tmpDir, srcPath);
            string absDstPath = String.Format("{0}/{1}/{2}", tmpDir, dstDir, dstFile);
            string absExpPath = String.Format("{0}/{1}", tmpDir, expFilePath);

            string cmdArgs = String.Format("-p {0}", absTmpSrcDir);
            Utils.Utils.Exec("mkdir", cmdArgs);

            cmdArgs = String.Format("-p {0}", absTmpDstDir);
            Utils.Utils.Exec("mkdir", cmdArgs);

            File.WriteAllText(absSrcPath, "helloworld");
            Utils.Utils.CopyFile(absSrcPath, absDstPath);

            bool fileExist = File.Exists(absExpPath);
            Assert.IsTrue(fileExist);
        }

        [TestCase("test1a", "test1a", "hello.txt", "test1b", "test1b/hello.txt", true)]
        [TestCase("test1a", "test1a/test1a-1", "hello.txt", "test1b", "test1b/test1a-1/hello.txt", true)]
        [TestCase("test1a", "test1a/1/2/3", "hello.txt", "test1b", "test1b/1/2/3/hello.txt", true)]
        [TestCase("test1a", "test1a/.git/", "hello.txt", "test1b", "test1b/.git/hello.txt", false)]
        public void CopyDirectoryTest(string topDir, string srcDir, string srcFile, string dstDir, string expFilePath, bool shouldFound)
        {
            Manifest mnf = new Manifest();
            Utils.Utils.SetManifest(mnf);

            string tmpDir = Path.GetTempPath();
            string absTmpSrcDir = String.Format("{0}/{1}", tmpDir, srcDir);
            string absTmpDstDir = String.Format("{0}/{1}", tmpDir, dstDir);
            string absTmpTopDir = String.Format("{0}/{1}", tmpDir, topDir);

            string absSrcPath = String.Format("{0}/{1}", absTmpSrcDir, srcFile);
            string absExpPath = String.Format("{0}/{1}", tmpDir, expFilePath);

            string cmdArgs = String.Format("-rf {0}", absTmpDstDir);
            Utils.Utils.Exec("rm", cmdArgs);

            cmdArgs = String.Format("-p {0}", absTmpSrcDir);
            Utils.Utils.Exec("mkdir", cmdArgs);

            cmdArgs = String.Format("-p {0}", absTmpDstDir);
            Utils.Utils.Exec("mkdir", cmdArgs);

            File.WriteAllText(absSrcPath, "helloworld");
            Utils.Utils.CopyDirectory(absTmpTopDir, absTmpDstDir);

            bool fileExist = File.Exists(absExpPath);
            Assert.AreEqual(shouldFound, fileExist);
        }

        [TestCase("ls", "-lrt abc")]
        public void ExeCmdWitheErrorTest(string cmd, string args)
        {
            Utils.Utils.Exec(cmd, args);
        }
    }
}