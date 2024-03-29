using System;
using System.IO;
using NUnit.Framework;
using Its.Iasc.Workflows.Utils;
using Its.Iasc.Copier;
using Its.Iasc.Cloners;

namespace Its.Iasc.Workflows
{
    public class WorkflowGenericTest
    {
        private string yaml1 = @"
config:
  defaultChartId: helm-terraform-gcp

vars:
  eckFile: all-in-one.yaml
  eckVersion: 1.5.0

charts:
  helm-terraform-gcp:
    chartUrl: https://its-software-services-devops.github.io/helm-terraform-gcp/
    version: 1.1.5-rc8
    params:
      - name: username
        value: ${SEC.HELM_USER}

      - name: password
        value: ${SEC.HELM_PASSWORD}

      - name: ca-file
        value: ${ENV.IASC_WIP_DIR}/harbor_aaa.pem

      - name: insecure-skip-tls-verify
        noValue: true
copy:
  - from: https://axxx/xxxxsdfsdfsd.com/sdfsdfsdf/ccccc.txt
    toFile: ccccc.txt
  - from: scripts/*.bash
    toDir: scripts
  - from: configs/*.yaml
    toDir: configs

infraIasc:
  - valuesFiles: [iasc-its-global.yaml, iasc-its-gce-manager.yaml, iasc-its-gce-rke.yaml]
    transformer: yaml2tf
    alias: global
    values:
    - --set-string a.bbbb.xxx=1234
    - --set a.bbbb.enabled=true

  - valuesFiles: [iasc-its-gce-rke.yaml]
    chartId: helm-terraform-gcp
    version: 1.1.5-rc8
    alias: iasc-its-global
    chartUrl: https://its-software-services-devops.github.io/helm-terraform-gcp/
    namespace: abcdefghi

  - valuesFiles: [iasc-its-gce-manager.yaml]
  - valuesFiles: [iasc-its-gce-rke.yaml]

  - valuesFiles: [iasc-its-gce-rke.yaml]
    chartId: helm-terraform-gcp
    version: 1.1.5-rc8
    alias: iasc-its-globalaaaa
    template: sshsksksk  
";
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void YamlParseNormalTest()
        {
            var wf = new WorkflowGeneric();
            var ctx = wf.GetContext();

            var result = wf.Load(yaml1);
            var m = wf.GetManifest();

            Assert.AreEqual(0, result);
            Assert.NotNull(m);
            Assert.NotNull(ctx);

            Assert.AreEqual("helm-terraform-gcp", m.Config.DefaultChartId);
            Assert.AreEqual(1, m.Charts.Count);
            Assert.AreEqual(5, m.InfraIasc.Length);

            var chart = m.Charts["helm-terraform-gcp"];

            Assert.AreEqual("https://its-software-services-devops.github.io/helm-terraform-gcp/", chart.ChartUrl);
            Assert.AreEqual("1.1.5-rc8", chart.Version);

            var iasc = m.InfraIasc[0];
            Assert.AreEqual("iasc-its-global.yaml", iasc.ValuesFiles[0]);
            Assert.AreEqual("helm-terraform-gcp", iasc.ChartId);
            Assert.AreEqual("1.1.5-rc8", iasc.Version);
            Assert.AreEqual("global", iasc.Alias);

            var nrmIasc = m.InfraIasc[2];
            Assert.AreEqual("iasc-its-gce-manager.yaml", nrmIasc.ValuesFiles[0]);
            Assert.AreEqual("helm-terraform-gcp", nrmIasc.ChartId);
            Assert.AreEqual("1.1.5-rc8", nrmIasc.Version);
            Assert.AreEqual("default-3", nrmIasc.Alias);
            Assert.AreEqual("https://its-software-services-devops.github.io/helm-terraform-gcp/", nrmIasc.ChartUrl);

            Assert.AreEqual("ccccc.txt", m.Copy[0].ToFile);
            Assert.AreEqual("configs", m.Copy[2].ToDir);          
        }

        private GenericCopier CreateGenericCopier()        
        {
            var cp = new GenericCopier();            
            cp.SetCopyCmd(CopyType.GsUtilCp, "echo");
            cp.SetCopyCmd(CopyType.Cp, "echo");
            cp.SetCopyCmd(CopyType.Http, "echo");

            return cp;
        }

        [Test]
        public void YamlTransformTest()
        {
            Environment.SetEnvironmentVariable("IASC_GSUTIL_PATH", "echo");

            var cn = new GitCloner();
            cn.SetGitCmd("echo");

            var wf = new WorkflowGeneric();
            wf.GetContext().SourceDir = ".";
            wf.GetContext().WipDir = ".";
            var result = wf.Load(yaml1);

            UtilsHelm.SetCmd("echo");
            wf.SetCopier(CreateGenericCopier());
            wf.SetCloner(cn);
            wf.Transform();
            UtilsHelm.ResetHelmCmd();
        }

        [Test]
        public void YamlLoadFileTest()
        {
            var path = "dummy.yaml";
            File.WriteAllText(path, yaml1);

            var cn = new GitCloner();
            cn.SetGitCmd("echo");

            var wf = new WorkflowGeneric();
            wf.GetContext().SourceDir = ".";
            wf.GetContext().WipDir = ".";
            wf.SetCloner(cn);
            wf.SetCopier(CreateGenericCopier());
            var result = wf.LoadFile(path);

            UtilsHelm.SetCmd("echo");
            wf.Transform();
            UtilsHelm.ResetHelmCmd();
        }

        [Test]
        public void CloneFileTest()
        {
            //To make code coverate 100%

            var cn = new GitCloner();
            cn.SetGitCmd("echo");

            var wf = new WorkflowGeneric();
            wf.GetContext().SourceDir = ".";
            wf.GetContext().WipDir = ".";
            wf.GetContext().VcsMode = "git";
            wf.SetCloner(cn);

            wf.CloneFiles();
        }        
    }
}