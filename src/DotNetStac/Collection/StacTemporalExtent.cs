using System;
using Newtonsoft.Json;

namespace Stac.Collection
{
    /// <summary>
    /// The class represents the temporal extents.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/collection-spec/collection-spec.md#temporal-extent-object">Temporal Extent Object</seealso>
    /// </summary>
    [JsonObject]
    public class StacTemporalExtent
    {


        /// <summary>
        /// Initialize a new instance of the <see cref="StacTemporalExtent" /> class with a single extent.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        [JsonConstructor]
        public StacTemporalExtent(DateTime? start, DateTime? end)
        {
            Interval = new DateTime?[1][] { new DateTime?[2] { start, end } };
        }

        /// <summary>
        /// Intialize a new Stac Temporal Extent from an exisiting one (clone)
        /// </summary>
        /// <param name="temporal"></param>
        public StacTemporalExtent(StacTemporalExtent temporal)
        {
            this.Interval = (System.DateTime?[][])temporal.Interval.Clone();
        }

        /// <summary>
        /// Potential temporal extents.
        /// </summary>
        /// <value></value>
        [JsonProperty("interval")]
        public DateTime?[][] Interval { get; set; }
    }
}
