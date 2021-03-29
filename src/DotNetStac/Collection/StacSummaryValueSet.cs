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

        public StacSummaryValueSet(JArray summarySet) : base(summarySet)
        {
        }

        public StacSummaryValueSet(IEnumerable<T> summarySet) : base(new JArray(summarySet))
        {
        }

        public void Add(T item){
            ((JArray)summary).Add(item);
        }

        public override SummaryItemType SummaryType => SummaryItemType.ValueSet;

        public int Count => summary.Count();

        public IEnumerable<T> SummarySet { get => summary.ToObject<List<T>>(); } 

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return SummarySet.GetEnumerator();
        }

    }
}