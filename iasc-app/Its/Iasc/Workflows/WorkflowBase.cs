using System;
using System.IO;
using System.Collections.Generic;
using Its.Iasc.Workflows.Utils;
using Its.Iasc.Workflows.Models;
using Its.Iasc.Transformers;
using Its.Iasc.Copier;
using Its.Iasc.Cloners;

namespace Its.Iasc.Workflows
{
    public abstract class WorkflowBase : IWorkflow
    {
        private Manifest manifest = null;
        private ICopier copier = new GenericCopier();

        private readonly Context ctx = new Context();
        private ICloner cloner = new GitCloner();

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

        private void SetupCopier()
        {
            copier.SetSrcDir(ctx.SourceDir);
            copier.SetWipDir(ctx.WipDir);

            var gsutilPath = Environment.GetEnvironmentVariable("IASC_GSTUIL_PATH");
            if (gsutilPath != null)
            {
                copier.SetCopyCmd(CopyType.GsUtilCp, gsutilPath);
            }

            copier.Process(manifest.Copy);
        }

        public void CloneFiles()
        {
            cloner.SetContext(ctx);
            cloner.Clone();
        }

        public int Transform()
        {
            Utils.Utils.SetManifest(manifest);
            UtilsHelm.SetSourceDir(ctx.SourceDir);

            SetupCopier();
            
            ITransformer xform = new DefaultTransformer(ctx);
            foreach (var iasc in manifest.InfraIasc)
            {
                UtilsHelm.HelmAdd(iasc);
                string output = UtilsHelm.HelmTemplate(iasc);

                var items = new List<string>();
                items.Add(output);

                //This can be replace by factory pattern                
                if (string.IsNullOrEmpty(iasc.Transformer))
                {
                    xform = new DefaultTransformer(ctx);
                }
                else if (iasc.Transformer.Equals("yaml2tf"))
                {
                    xform = new Yaml2Terraform(ctx);
                } 
                xform.Transform(items, iasc);

                items.Clear();                           
            }

            return 0;
        }

        public Manifest GetManifest()
        {
            return manifest;
        }

        public void SetCopier(ICopier cp)
        {
            copier = cp;
        }

        public void SetCloner(ICloner cl)
        {
            cloner = cl;
        }        
    }
}