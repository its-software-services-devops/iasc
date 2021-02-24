using CommandLine;

namespace Its.Iasc.Options
{
    [Verb("init", HelpText = " Initialize a new or existing Terraform working directory.")]
    public class InitOptions
    {
        [Option('v', "verbose", Required = true, HelpText = "Set output to verbose messages.")]
        public string Verbose { get; set; }
    }    
}

