using CommandLine;
using Its.Iasc.Options;
using Its.Iasc.Actions;

namespace Its.Iasc
{
    class Program
    {
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions>(args)
                .WithParsed<InitOptions>(ActionUtils.RunInitAction)
                .WithParsed<PlanOptions>(ActionUtils.RunPlanAction)
                .WithParsed<ApplyOptions>(ActionUtils.RunApplyAction);
        }
    }
}
