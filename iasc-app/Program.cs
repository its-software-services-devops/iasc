using Its.Iasc.Options;
using CommandLine;

namespace its.iasc
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions>(args);
        }
    }
}
