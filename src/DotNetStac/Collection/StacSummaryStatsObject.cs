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
            Min = summary["min"].Value<T>();
            Max = summary["max"].Value<T>();
        }

        public StacSummaryStatsObject(T min, T max) : base(null)
        {
            Min = min;
            Max = max;
        }

        public T Min { get; set; }

        public T Max { get; set; }

        public override SummaryItemType SummaryType => SummaryItemType.StatsObject;


    }
}