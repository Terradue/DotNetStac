using Newtonsoft.Json;

namespace Stac.Collection
{
    /// <summary>
    /// The class represents the spatial extents.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/collection-spec/collection-spec.md#spatial-extent-object">Spatial Extent Object</seealso>
    /// </summary>
    [JsonObject]
    public class StacSpatialExtent
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="StacSpatialExtent" /> class with a single extent.
        /// </summary>
        /// <param name="minX">Minimum X bound</param>
        /// <param name="minY">Minimum Y bound</param>
        /// <param name="maxX">Maximum X bound</param>
        /// <param name="maxY">Maximum Y bound</param>
        [JsonConstructor]
        public StacSpatialExtent(double minX, double minY, double maxX, double maxY)
        {
            BoundingBoxes = new double[1][] { new double[4] { minX, minY, maxX, maxY } };
        }

        /// <summary>
        /// Potential spatial extents.
        /// </summary>
        /// <value>Gets/sets double entry array of coordinates</value>
        [JsonProperty("bbox")]
        public double[][] BoundingBoxes { get; set; }
    }
}
