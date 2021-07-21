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
ONLYHERE=aaaaaasdddd
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

        private const string secret4 = @"
---
USERNAME=ext1
PASSWORD=passwordExt1
SPECIFIC=adsdfsdf
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
            Vault.Clear();

            var path = String.Format("{0}/{1}", Path.GetTempPath(), "secretfile_VaultGetValueTest.txt");
            File.WriteAllText(path, content);

            Vault.Setup(path, null);
            Vault.Load();

            string secretValue = Vault.GetValue(key);
            Assert.AreEqual(expValue, secretValue);
        }

        [TestCase(secret3, secret4, "USERNAME", "ext1")]
        [TestCase(secret3, secret4, "PASSWORD", "passwordExt1")]
        [TestCase(secret3, secret4, "SPECIFIC", "adsdfsdf")]
        [TestCase(secret1, secret4, "ONLYHERE", "aaaaaasdddd")]
        public void VaultWithExtGetValueTest(string content1, string content2, string key, string expValue)
        {
            Vault.Clear();

            var path1 = String.Format("{0}/{1}", Path.GetTempPath(), "secretfile_VaultGetValueTest1.txt");
            File.WriteAllText(path1, content1);
            
            var path2 = String.Format("{0}/{1}", Path.GetTempPath(), "secretfile_VaultGetValueTest2.txt");
            File.WriteAllText(path2, content2);

            Vault.Setup(path1, path2);
            Vault.Load();

            string secretValue = Vault.GetValue(key);
            Assert.AreEqual(expValue, secretValue);
        }

        [TestCase("USERNAME", null)]
        public void VaultNoLoadGetValueTest(string key, string expValue)
        {
            Vault.Clear();
            Vault.Load();

            string secretValue = Vault.GetValue(key);
            Assert.AreEqual(expValue, secretValue);
        }           
    }
}