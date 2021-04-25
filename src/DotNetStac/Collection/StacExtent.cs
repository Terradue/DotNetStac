using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stac.Collection
{
    [JsonObject]
    public class StacExtent
    {
        [JsonProperty("spatial")]
        public StacSpatialExtent Spatial { get; set; }

        [JsonProperty("temporal")]
        public StacTemporalExtent Temporal { get; set; }

        public static StacExtent Create(IEnumerable<StacItem> items)
        {
            return new StacExtent()
            {
                Spatial = new StacSpatialExtent(items.Min(i => i.GetBoundingBoxFromGeometryExtent()[0]),
                                                items.Min(i => i.GetBoundingBoxFromGeometryExtent()[1]),
                                                items.Max(i => i.GetBoundingBoxFromGeometryExtent()[2]),
                                                items.Max(i => i.GetBoundingBoxFromGeometryExtent()[3])),
                Temporal = new StacTemporalExtent(items.Min(i => i.CommonMetadata().DateTime.Start), items.Max(i => i.CommonMetadata().DateTime.End))
            };
        }
    }
}