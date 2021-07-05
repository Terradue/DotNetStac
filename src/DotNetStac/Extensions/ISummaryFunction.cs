using System.Collections.Generic;
using Stac.Collection;

namespace Stac.Extensions
{
    public interface ISummaryFunction
    {
        IStacExtension Extension { get; }

        string PropertyName { get; }

        IStacSummaryItem Summarize(IEnumerable<object> items);
    }
}
