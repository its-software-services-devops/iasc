using System.Collections.Generic;
using Its.Iasc.Workflows;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public abstract class BaseTransformer : ITransformer
    {
        private readonly Context context = null;
        private Manifest manifest = null;

        public abstract IList<string> Transform(IList<string> items, Infra cfg);

        public void SetManifest(Manifest mnf)
        {
            manifest = mnf;
        }
        
        protected Context GetContext()
        {
            return context;
        }

        public BaseTransformer(Context ctx)
        {
            context = ctx;
        }
    }
}