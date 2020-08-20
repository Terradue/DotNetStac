using System;
using Newtonsoft.Json;

namespace Stac
{
    [JsonObject]
    public class StacTemporalExtent
    {
        public StacTemporalExtent()
        {
        }

        public StacTemporalExtent(DateTime? start, DateTime? end)
        {
            Interval = new DateTime?[1][] { new DateTime?[2] { start, end } };
        }

        [JsonProperty("interval")]
        public DateTime?[][] Interval { get; set; }
    }
}