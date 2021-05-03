using System.Collections.Generic;

namespace Its.Iasc.Workflows.Models
{
    public class Manifest
    {
        public GlobalConfig Config { get; set; } 
        public Dictionary<string, HelmChart> Charts { get; set; }
        public CopyItem[] Copy { get; set; }
        public Dictionary<string, string> Vars { get; set; }
        public Infra[] InfraIasc { get; set; }
    } 
}
