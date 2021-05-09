using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Vaults
{
    public class VaultProviderSecretFileTest
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

        private ICommandExecutor CreateMockedExecutor(string valueNeed)
        {
            var exec = new Mock<ICommandExecutor>();
            exec.Setup(x=>x.Exec(It.IsAny<string>(), It.IsAny<string>())).Returns(valueNeed);

            return exec.Object;
        }        

        [TestCase(secret1, "USERNAME", "test1")]
        [TestCase(secret1, "PASSWORD", "password1")]
        [TestCase(secret1, "NOTFOUND", null)]
        [TestCase(secret2, "PASSWORD", "password2")]
        [TestCase(secret3, "DATA", "equalinp=assword2")]
        public void VaultGetValueTest(string content, string key, string expValue)
        {            
            ICommandExecutor exec = CreateMockedExecutor(content);

            var vc = new VaultConfig();
            vc.SecretFileName = "gs://dummy/secrets.txt";

            var v = new VaultProviderSecretFile(vc);
            v.SetCmdExecutor(exec);
            v.SetGsutilCmd("gsutil");
            v.Load();

            string secretValue = v.GetValue(key);
            Assert.AreEqual(expValue, secretValue);
        }
    }
}