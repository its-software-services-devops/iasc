using CommandLine;

namespace Its.Iasc.Options
{
    [Verb("plan", HelpText = "Generates a speculative execution plan, showing what actions Terraform would take to apply the current configuration.")]
    public class PlanOptions
    {
    }    
}
