using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public class Yaml2Terraform : ITransformer
    {
        private readonly Context context = null;

        public Yaml2Terraform(Context ctx)
        {
            context = ctx;
        }

        protected void ProcessLines(List<string> lines, Infra cfg)
        {
            string currentFname = "";
            var contents = new List<string>();

            Regex breakRegex = new Regex(@"^---$");
            Regex cfgMapRegex = new Regex(@"^(.+): \|\s*$"); //02_gce_rke.tf: |

            foreach (string line in lines)
            {
                if (breakRegex.IsMatch(line))
                {
                    //Skipping the ---
                    if (!string.IsNullOrEmpty(currentFname))
                    {
                        UtilsTransformer.WriteFileContent(context, currentFname, contents);
                        contents.Clear();
                    }
                }
                else if (cfgMapRegex.IsMatch(line))
                {
                    var match = cfgMapRegex.Match(line);
                    string file = match.Groups[1].Value;

                    currentFname = String.Format("{0}_{1}", cfg.Alias, file);                    
                }
                else
                {
                    contents.Add(line);
                }
            }

            if (contents.Count > 0)
            {
                UtilsTransformer.WriteFileContent(context, currentFname, contents);
            }
        }

        public IList<string> Transform(IList<string> items, Infra cfg)
        {
            var lines = UtilsTransformer.MultiLinesToArray(items);
            ProcessLines(lines, cfg);

            return lines;
        }
    }
}