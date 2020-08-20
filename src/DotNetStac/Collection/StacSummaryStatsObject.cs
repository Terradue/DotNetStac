using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    public class StacSummaryStatsObject<T> : StacSummaryItem
    {
        public StacSummaryStatsObject(JObject summary) : base(summary)
        {
            if (!summary.ContainsKey("min") || !summary.ContainsKey("max"))
                throw new ArgumentException("summary stats must contains min and max");
        }

        public StacSummaryStatsObject(T min, T max) : base(new JObject())
        {
            Min = min;
            Max = max;
        }

        public T Min { get => summary["min"].Value<T>(); set => summary["min"] = new JValue(value); }

        public T Max { get => summary["max"].Value<T>(); set => summary["max"] = new JValue(value); }

        public override SummaryItemType SummaryType => SummaryItemType.StatsObject;


    }
}