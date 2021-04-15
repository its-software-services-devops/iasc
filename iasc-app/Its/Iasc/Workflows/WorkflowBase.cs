using System.IO;
using Its.Iasc.Workflows.Utils;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows
{
    public abstract class WorkflowBase : IWorkflow
    {
        private Manifest manifest = null;
        private Context ctx = new Context();
        
        protected abstract Manifest ParseManifest(string manifestContent);

        public int Load(string manifestContent)
        {
            manifest = UtilsManifest.Normalize(ParseManifest(manifestContent));
            return 0;
        }
        public int LoadFile(string fileName)
        {
            Directory.SetCurrentDirectory(ctx.SourceDir);
            string readText = File.ReadAllText(fileName);
            Load(readText);
          
            return 0;
        }

        public Context GetContext()
        {
            return ctx;
        }

        public int Transform()
        {
            foreach (var iasc in manifest.InfraIasc)
            {
                UtilsHelm.HelmAdd(iasc);
                UtilsHelm.HelmTemplate(iasc);
            }

            return 0;
        }

        public Manifest GetManifest()
        {
            return manifest;
        }
    }
}