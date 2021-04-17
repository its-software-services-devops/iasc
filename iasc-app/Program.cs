using Serilog;
using CommandLine;
using Its.Iasc.Options;
using Its.Iasc.Actions;

namespace Its.Iasc
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;

            Log.Information("Running [iasc] version [1.1.0]");

            Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions>(args)
                .WithParsed<InitOptions>(UtilsAction.RunInitAction)
                .WithParsed<PlanOptions>(UtilsAction.RunPlanAction)
                .WithParsed<ApplyOptions>(UtilsAction.RunApplyAction);
        }
    }
}
