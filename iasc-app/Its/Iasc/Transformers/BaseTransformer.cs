using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public abstract class BaseTransformer : ITransformer
    {
        private readonly Context context = null;
        public abstract IList<string> Transform(IList<string> items, Infra cfg);
        
        protected Context GetContext()
        {
            return context;
        }

        protected BaseTransformer(Context ctx)
        {
            context = ctx;
        }
    }
}