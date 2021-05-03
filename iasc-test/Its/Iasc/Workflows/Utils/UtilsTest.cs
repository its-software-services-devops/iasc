using System;
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
    }
}