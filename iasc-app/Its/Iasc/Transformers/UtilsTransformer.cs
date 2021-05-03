using System;
using System.IO;
using System.Collections.Generic;
using Serilog;
using Its.Iasc.Workflows;

namespace Its.Iasc.Transformers
{
    public static class UtilsTransformer
    {
        public static void WriteFileContent(Context context, string fname, List<string> lines)
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
