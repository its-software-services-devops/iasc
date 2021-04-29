using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public class Yaml2TerraformTest
    {
        private string yaml1 = @"
---
02_gce_rke.tf: |       
  config:
    defaultChartId: helm-terraform-gcp

  charts:
    helm-terraform-gcp:
      chartUrl: https://its-software-services-devops.github.io/helm-terraform-gcp/
      version: 1.1.5-rc8

  infraIasc:
    - valuesFile: iasc-its-global.yaml
      chartId: helm-terraform-gcp
      version: 1.1.5-rc8
      alias: iasc-its-global
      chartUrl: https://its-software-services-devops.github.io/helm-terraform-gcp/

    - valuesFile: iasc-its-gce-manager.yaml
    - valuesFile: iasc-its-gce-rke.yaml
---
02_gce_aaaaa.tf: |       
  config:
    defaultChartId: helm-terraform-gcp
";
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("")]
        [TestCase("temp")]
        public void TransformTestWithoutDir(string wipDir)
        {
            //Only one "02_gce_rke.tf: |" per block

            var ctx = new Context();

            ctx.WipDir = wipDir;
            if (!string.IsNullOrEmpty(wipDir))
            {
                ctx.WipDir = Path.GetTempPath();
            }
            
            var cfg = new Infra();
            var tfm = new Yaml2Terraform(ctx);

            var lines = new List<string>();
            lines.Add(yaml1);

            tfm.Transform(lines, cfg);
        }
    }
}