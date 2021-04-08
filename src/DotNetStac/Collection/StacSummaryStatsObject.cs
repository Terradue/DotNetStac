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
            if (!summary.ContainsKey("minimum") || !summary.ContainsKey("maximum"))
                throw new ArgumentException("summary stats must contains minimum and maximum fields");
        }

        public StacSummaryStatsObject(T min, T max) : base(new JObject())
        {
            Min = min;
            Max = max;
        }

        public T Min { get => summary["minimum"].Value<T>(); set => summary["minimum"] = new JValue(value); }

        public T Max { get => summary["maximum"].Value<T>(); set => summary["maximum"] = new JValue(value); }


    }
}