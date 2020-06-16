using System;
using System.Collections;

namespace Stac.Collection
{
    public abstract class StacSummaryItem<T> : IStacSummaryItem
    {
        public abstract SummaryItemType SummaryType { get; }
        public abstract Type ValueType { get; }

        public abstract IEnumerator GetEnumerator();
    }
}