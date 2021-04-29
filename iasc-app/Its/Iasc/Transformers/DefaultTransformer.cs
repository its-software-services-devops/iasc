using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public class DefaultTransformer : ITransformer
    {
        public DefaultTransformer(Context ctx)
        {
        }

        public IList<string> Transform(IList<string> items, Infra cfg)
        {
            var lines = new List<string>();
            return lines;
        }
    }
}