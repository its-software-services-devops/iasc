using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows.Utils
{
    public static class UtilsManifest
    {
        public static Manifest Normalize(Manifest manifest)
        {
            string defaultChart = manifest.Config.DefaultChartId;
            var chart = manifest.Charts[defaultChart];

            int cnt = 0;
            foreach (var iasc in manifest.InfraIasc)
            {
                cnt++;

                if (iasc.ChartId == null)
                {
                    iasc.ChartId = defaultChart;
                }

                if (iasc.ChartUrl == null)
                {
                    iasc.ChartUrl = chart.ChartUrl;
                }

                if (iasc.Version == null)
                {
                    iasc.Version = chart.Version;
                }

                if (iasc.Alias == null)
                {
                    iasc.Alias = string.Format("default-{0}", cnt);
                }                                
            }

            return manifest;
        }
    }
}
