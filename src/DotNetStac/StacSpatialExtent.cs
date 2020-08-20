using Newtonsoft.Json;

namespace Stac
{
    [JsonObject]
    public class StacSpatialExtent
    {
        public StacSpatialExtent()
        {
        }

        public StacSpatialExtent(double minX, double minY, double maxX, double maxY)
        {
            BoundingBoxes = new double[1][] { new double[4] { minX, minY, maxX, maxY } };
        }

        [JsonProperty("bbox")]
        public double[][] BoundingBoxes { get; set; }
    }
}