using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Stac.Collection;

namespace Stac.Collection
{
    public class StacSummaryValueSet<T> : StacSummaryItem<T>
    {
        private IEnumerable<T> enumerable;

        [JsonConstructor]
        public StacSummaryValueSet(IEnumerable<T> enumerable)
        {
            this.enumerable = new List<T>(enumerable);
        }

        public override Type ValueType => typeof(T);

        public override SummaryItemType SummaryType => SummaryItemType.Set;

        public override IEnumerator GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }
    }
}