using System;
using Its.Iasc.Options;
using Its.Iasc.Workflows;

namespace Its.Iasc.Actions
{
    public class ActionInit : BaseAction
    {
        private WorkflowGeneric wf = new WorkflowGeneric();

        public ActionInit()
        {
            wf.GetContext().SourceDir = "samples/";
        }

        protected override int RunAction(BaseOptions options)
        {
            Console.WriteLine("Action = [Init] Verbose = [{0}]", options.Verbosity); 
            //var result = wf.LoadFile("manifest.yaml");
            //wf.Transform();

            return 0;
        }
    }
}