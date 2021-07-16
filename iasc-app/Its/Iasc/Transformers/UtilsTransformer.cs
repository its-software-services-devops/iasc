using System;
using System.IO;
using System.Collections.Generic;
using Serilog;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Utils;

namespace Its.Iasc.Transformers
{
    public static class UtilsTransformer
    {
        public static string ConstructPath(string baseDir, string subDir, string fname)
        {
            string outDir = "";            
            if (!string.IsNullOrEmpty(baseDir) && !string.IsNullOrEmpty(subDir))
            {
                outDir = String.Format("{0}/{1}", baseDir, subDir);
            }
            else if (!string.IsNullOrEmpty(baseDir))
            {
                outDir = baseDir;
            }
            else if (!string.IsNullOrEmpty(subDir))
            {
                outDir = subDir;
            }

            string path = fname;
            if (!string.IsNullOrEmpty(outDir))
            {
                Utils.Exec("mkdir", String.Format("-p {0}", outDir));
                path = String.Format("{0}/{1}", outDir, fname);
            }

            return path;
        }

        public static void WriteFileContent(Context context, string fname, List<string> lines, string toDir)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return;
            }
            
            string path = ConstructPath(context.WipDir, toDir, fname);                        
            int cnt = lines.Count;

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                lines.ForEach(r=>sw.WriteLine(r));

                sw.Flush();
            }

            Log.Information("Wrote {0} line(s) to file [{1}]", cnt, path);
        }

        public static List<string> MultiLinesToArray(IList<string> items)
        {
            var lines = new List<string>();

            char[] delims = new[] { '\r', '\n' };
            foreach (string item in items)
            {
                string[] tokens = item.Split(delims, StringSplitOptions.RemoveEmptyEntries);
                lines.AddRange(tokens);
            }

            return lines;       
        } 
    }
}
