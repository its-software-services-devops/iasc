using CommandLine;
using Its.Iasc.Options;
using Its.Iasc.Actions;

namespace Its.Iasc
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions>(args)
                .WithParsed<InitOptions>(UtilsAction.RunInitAction)
                .WithParsed<PlanOptions>(UtilsAction.RunPlanAction)
                .WithParsed<ApplyOptions>(UtilsAction.RunApplyAction);
        }
    }
}
