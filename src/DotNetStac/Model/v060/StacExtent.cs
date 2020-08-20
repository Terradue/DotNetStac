using System;
using Newtonsoft.Json;

namespace Stac.Model.v060
{
    [JsonObject]
    internal class StacExtent060
    {
        [JsonProperty("spatial")]
        public double[] Spatial { get; set; }

        [JsonProperty("temporal")]
        public DateTime?[] Temporal { get; set; }

        internal StacExtent Upgrade()
        {
            return new StacExtent(){
                Spatial = new StacSpatialExtent(this.Spatial[0], this.Spatial[1], this.Spatial[2], this.Spatial[3]),
                Temporal = new StacTemporalExtent(this.Temporal[0], this.Temporal[1])
            };
        }
    }
}