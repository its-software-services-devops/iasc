using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;
using Serilog;

namespace Its.Iasc.Transformers
{
    public class DefaultTransformer : ITransformer
    {
        private readonly Context context = null;

        public DefaultTransformer(Context ctx)
        {
            context = ctx;
        }

        public IList<string> Transform(IList<string> items, Infra cfg)
        {
            var lines = new List<string>();
            return lines;
        }
    }
}