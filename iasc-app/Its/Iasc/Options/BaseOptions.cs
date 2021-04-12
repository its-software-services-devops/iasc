using CommandLine;

namespace Its.Iasc.Options
{
    public class BaseOptions
    {
        [Option('v', "verbosity", Required = false, HelpText = "Set output to verbose messages.")]
        public string Verbosity { get; set; }        
    }    
}