using Serilog;
using System.Reflection;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionInfo : BaseAction
    {
        protected override int RunAction(BaseOptions options)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyVersion = assembly.GetName().Version;

            Log.Information("Version = [{0}]", assemblyVersion); 
            return 0;
        }
    }
}