using System;
using Serilog;
using CommandLine;
using System.Reflection;
using Its.Iasc.Options;
using Its.Iasc.Actions;
using Its.Iasc.Vaults;
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

            Vault.Setup(Environment.GetEnvironmentVariable("IASC_VAULT_CONFIG"));
            Vault.Load();            

            Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions, InfoOptions>(args)
                .WithParsed<InitOptions>(UtilsAction.RunInitAction)
                .WithParsed<PlanOptions>(UtilsAction.RunPlanAction)
                .WithParsed<ApplyOptions>(UtilsAction.RunApplyAction)
                .WithParsed<InfoOptions>(UtilsAction.RunInfoAction);
        }
    }
}
