using System.Collections.Generic;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Transformers
{
    public interface ITransformer
    {
        IList<string> Transform(IList<string> items, Infra cfg);
    }
}