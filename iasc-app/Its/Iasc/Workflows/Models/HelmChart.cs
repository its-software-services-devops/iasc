using System;

namespace Its.Iasc.Workflows.Models
{
    public class HelmChart
    {
        public string ChartUrl { get; set; }
        public string Version { get; set; }
        public CmdParam[] Params { get; set; }

        public HelmChart()
        {
            Params = Array.Empty<CmdParam>();
        }
    }     
}

