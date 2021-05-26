using System;
using System.IO;
using NUnit.Framework;

namespace Its.Iasc.Vaults
{
    public class VaultTest
    {
        private const string secret1 = @"
---
USERNAME=test1
PASSWORD=password1
";

        private const string secret2 = @"
---
USERNAME=test1
PASSWORD=password1
PASSWORD=password2
";

        private const string secret3 = @"
---
USERNAME=test1
PASSWORD=password1
DATA=equalinp=assword2
";

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(secret1, "USERNAME", "test1")]
        [TestCase(secret1, "PASSWORD", "password1")]
        [TestCase(secret1, "NOTFOUND", null)]
        [TestCase(secret2, "PASSWORD", "password2")]
        [TestCase(secret3, "DATA", "equalinp=assword2")]
        public void VaultGetValueTest(string content, string key, string expValue)
        {
            var path = String.Format("{0}/{1}", Path.GetTempPath(), "secretfile_VaultGetValueTest.txt");
            File.WriteAllText(path, content);

            Vault.Setup(path);
            Vault.Load();

            string secretValue = Vault.GetValue(key);
            Assert.AreEqual(expValue, secretValue);
        }
    }
}