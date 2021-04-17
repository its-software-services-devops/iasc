using Serilog;
using System.Reflection;
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

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyVersion = assembly.GetName().Version;
            Log.Information("Running [iasc] version [{0}]", assemblyVersion);

            Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions>(args)
                .WithParsed<InitOptions>(UtilsAction.RunInitAction)
                .WithParsed<PlanOptions>(UtilsAction.RunPlanAction)
                .WithParsed<ApplyOptions>(UtilsAction.RunApplyAction);
        }
    }
}
