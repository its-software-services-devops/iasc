using System;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionApply : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            Console.WriteLine("Action = [Apply] Verbose = [{0}]", options.Verbosity);
            return 0;
        }
    }
}