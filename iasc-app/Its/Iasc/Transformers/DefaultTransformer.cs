using System;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public class DefaultTransformer : BaseTransformer
    {
        public DefaultTransformer(Context ctx) : base(ctx)
        {
        }

        public override IList<string> Transform(IList<string> items, Infra cfg)
        {
            var lines = UtilsTransformer.MultiLinesToArray(items);
            UtilsTransformer.WriteFileContent(GetContext(), String.Format("{0}.yaml", cfg.Alias), lines, cfg.ToDir);

            return lines;
        }
    }
}