using System;
using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;


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
            var lines = UtilsTransformer.MultiLinesToArray(items);
            UtilsTransformer.WriteFileContent(context, String.Format("{0}.yaml", cfg.Alias), lines);

            return lines;
        }
    }
}