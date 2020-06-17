using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Collection;

namespace Stac.Collection
{
    public class StacSummaryValueSet<T> : StacSummaryItem, IEnumerable<T>
    {
        private readonly List<T> summarySet;

        public StacSummaryValueSet(JArray summarySet) : base(summarySet)
        {
            this.summarySet = summarySet.ToObject<List<T>>();
        }

        public override SummaryItemType SummaryType => SummaryItemType.Set;

        public int Count => summary.Count();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return summarySet.GetEnumerator();
        }

    }
}