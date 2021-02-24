using System;
using CommandLine;
using Its.Iasc.Options;

namespace its.iasc
{
    class Program
    {
        private static void RunInit(InitOptions o)
        {
            Console.WriteLine("Verbose = [{0}]", o.Verbose);
        }

        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<InitOptions, PlanOptions, ApplyOptions>(args)
                .WithParsed<InitOptions>(RunInit);
        }
    }
}
