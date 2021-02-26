using System;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionInit : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            Console.WriteLine("Action = [Init] Verbose = [{0}]", options.Verbosity);
            return 0;
        }
    }
}