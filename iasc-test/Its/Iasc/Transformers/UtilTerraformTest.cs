using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public class UtilTerraformTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("/basedir", "a/b/c", "name", "/basedir/a/b/c/name")]
        [TestCase("", "a/b/c", "name", "a/b/c/name")]
        [TestCase("/basedir", "", "name", "/basedir/name")]
        [TestCase("", "", "name", "name")]
        public void ConstructPathTest(string baseDir, string subDir, string fname, string expected)
        {
            string path = UtilsTransformer.ConstructPath(baseDir, subDir, fname);
            Assert.AreEqual(expected, path);
        }
    }
}