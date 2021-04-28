using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;
using Serilog;

namespace Its.Iasc.Transformers
{
    public class Yaml2Terraform : ITransformer
    {
        private readonly Context context = null;

        public Yaml2Terraform(Context ctx)
        {
            context = ctx;
        }

        private void WriteFileContent(string fname, List<string> lines)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return;
            }
            
            string path = fname;
            if (!context.WipDir.Equals(""))
            {
                path = String.Format("{0}/{1}", context.WipDir, fname);
            }
            
            int cnt = lines.Count;

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                lines.ForEach(r=>sw.WriteLine(r));

                sw.Flush();
            }

            Log.Information("Wrote {0} line(s) to file [{1}]", cnt, path);
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
                WriteFileContent(currentFname, contents);
            }
        }

        public IList<string> Transform(IList<string> items, Infra cfg)
        {
            var lines = new List<string>();

            char[] delims = new[] { '\r', '\n' };
            foreach (string item in items)
            {
                string[] tokens = item.Split(delims, StringSplitOptions.RemoveEmptyEntries);
                lines.AddRange(tokens);
            }

            ProcessLines(lines, cfg);

            return lines;
        }
    }
}