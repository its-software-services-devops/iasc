using Serilog;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionPlan : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            Log.Information("Action = [Plan] Verbose = [{0}]", options.Verbosity); 
            return 0;
        }
    }
}