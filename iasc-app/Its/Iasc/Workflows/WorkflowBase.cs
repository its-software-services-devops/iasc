
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows
{
    public abstract class WorkflowBase : IWorkflow
    {
        private Manifest manifest = null;
        
        protected abstract Manifest ParseManifest(string manifestContent);

        public int Load(string manifestContent)
        {
            manifest = ParseManifest(manifestContent);
            return 0;
        }

        public Manifest GetManifest()
        {
            return manifest;
        }
    }
}