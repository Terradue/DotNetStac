using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stac.Collection
{
    /// <summary>
    /// The class describes the spatio-temporal extents of the Collection, both spatial and temporal.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/collection-spec/collection-spec.md#extent-object">STAC Extent Object</seealso>
    /// </summary>
    [JsonObject]
    public class StacExtent
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="StacExtent" /> class.
        /// </summary>
        /// <param name="spatial">Spatial Extent</param>
        /// <param name="temporal">Temporal Extent</param>
        public StacExtent(StacSpatialExtent spatial, StacTemporalExtent temporal)
        {
            Spatial = spatial;
            Temporal = temporal;
        }

        /// <summary>
        /// Potential <see cref="StacSpatialExtent" /> covered by the Collection.
        /// </summary>
        /// <value>Gets/sets the spatial extent</value>
        [JsonProperty("spatial")]
        public StacSpatialExtent Spatial { get; set; }

        /// <summary>
        /// Potential <see cref="StacTemporalExtent" /> covered by the Collection.
        /// </summary>
        /// <value>Gets/sets the temporal extent</value>
        [JsonProperty("temporal")]
        public StacTemporalExtent Temporal { get; set; }

        /// <summary>
        /// Create a new <see cref="StacExtent" /> from a set of <see cref="StacItem" />
        /// </summary>
        /// <param name="items">Set of <see cref="StacItem" /></param>
        /// <returns>A <see cref="StacExtent" /> that represents the spatio-temporal extent of all the items together</returns>
        public static StacExtent Create(IEnumerable<StacItem> items)
        {
            return new StacExtent(
                new StacSpatialExtent(items.Min(i => i.GetBoundingBoxFromGeometryExtent()[0]),
                                                items.Min(i => i.GetBoundingBoxFromGeometryExtent()[1]),
                                                items.Max(i => i.GetBoundingBoxFromGeometryExtent()[2]),
                                                items.Max(i => i.GetBoundingBoxFromGeometryExtent()[3])),
                new StacTemporalExtent(items.Min(i => i.CommonMetadata().DateTime.Start), items.Max(i => i.CommonMetadata().DateTime.End))
            );
        }
    }
}