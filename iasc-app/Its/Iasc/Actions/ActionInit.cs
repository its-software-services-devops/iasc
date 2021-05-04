using Its.Iasc.Options;

using Serilog;
namespace Its.Iasc.Actions
{
    public class ActionInit : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            Log.Information("Action = [Init] Verbose = [{0}]", options.Verbosity); 

            var wf = GetWorkflow();
            wf.CloneFiles();
            wf.LoadFile("manifest.yaml");
            wf.Transform();

            return 0;
        }
    }
}