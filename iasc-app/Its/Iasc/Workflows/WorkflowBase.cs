using System;
using System.IO;
using System.Collections.Generic;
using Its.Iasc.Workflows.Utils;
using Its.Iasc.Workflows.Models;
using Its.Iasc.Transformers;

namespace Its.Iasc.Workflows
{
    public abstract class WorkflowBase : IWorkflow
    {
        private Manifest manifest = null;
        private readonly Context ctx = new Context();
        
        protected abstract Manifest ParseManifest(string manifestContent);

        public int Load(string manifestContent)
        {
            manifest = UtilsManifest.Normalize(ParseManifest(manifestContent));
            return 0;
        }
        public int LoadFile(string fileName)
        {
            string inputPath = String.Format("{0}/{1}", ctx.SourceDir, fileName);
            
            string readText = File.ReadAllText(inputPath);
            Load(readText);
          
            return 0;
        }

        public Context GetContext()
        {
            return ctx;
        }

        public int Transform()
        {
            UtilsHelm.SetSourceDir(ctx.SourceDir);
            
            foreach (var iasc in manifest.InfraIasc)
            {
                UtilsHelm.HelmAdd(iasc);
                string output = UtilsHelm.HelmTemplate(iasc);

                var items = new List<string>();
                items.Add(output);

                var xform = new Yaml2Terraform(ctx);
                xform.Transform(items, iasc);
            }

            return 0;
        }

        public Manifest GetManifest()
        {
            return manifest;
        }
    }
}