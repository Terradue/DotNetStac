using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    [JsonObject]
    public class StacSummaryStatsObject<T> : StacSummaryItem<T>
    {
        [JsonProperty("min")]
        public T Min { get; set; }

        [JsonProperty("max")]
        public T Max { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> Extensions { get; set; }

        public override SummaryItemType SummaryType => SummaryItemType.StatsObject;

        public override Type ValueType => typeof(T);

        public override IEnumerator GetEnumerator()
        {
            foreach (T value in new T[2] { Min, Max })
            {
                yield return value;
            }
        }
    }
}