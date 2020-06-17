using Newtonsoft.Json;

namespace Stac
{
    [JsonObject]
    public class StacExtent
    {
        [JsonProperty("spatial")]
        public StacSpatialExtent Spatial { get; set; }

        [JsonProperty("temporal")]
        public StacTemporalExtent Temporal { get; set; }
    }
}