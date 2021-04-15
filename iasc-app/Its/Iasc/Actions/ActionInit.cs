using System;
using Its.Iasc.Options;
using Its.Iasc.Workflows;

namespace Its.Iasc.Actions
{
    public class ActionInit : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            Console.WriteLine("Action = [Init] Verbose = [{0}]", options.Verbosity); 

            var wf = GetWorkflow();
            wf.GetContext().SourceDir = "samples/";

            wf.LoadFile("manifest.yaml");
            wf.Transform();

            return 0;
        }
    }
}