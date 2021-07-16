using System;

namespace Its.Iasc.Workflows.Models
{
    public class Infra
    {
        public string[] ValuesFiles { get; set; }
        public string[] Values { get; set; }
        public string Transformer { get; set; }
        public string ChartId { get; set; }
        public string ChartUrl { get; set; }
        public string Version { get; set; }
        public string Alias { get; set; }
        public string Namespace { get; set; }
        public CmdParam[] ChartParams { get; set; }
        public string ToDir { get; set; }

        public Infra()
        {
            Values = Array.Empty<string>();
            ValuesFiles = Array.Empty<string>();
            ChartParams = Array.Empty<CmdParam>();
        }         
    } 
}

