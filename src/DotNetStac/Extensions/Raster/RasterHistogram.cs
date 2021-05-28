using Newtonsoft.Json;

namespace Stac.Extensions.Raster
{
    /// <summary>
    /// The histogram object provides with distribution of pixel values in the band. Those values are sampled in buckets.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RasterHistogram
    {
        /// <summary>
        /// number of buckets of the distribution.
        /// </summary>
        /// <value></value>
        [JsonProperty("count")]
        public int? Count { get; set; }

        /// <summary>
        /// minimum value of the distribution. Also the mean value of the first bucket.
        /// </summary>
        /// <value></value>
        [JsonProperty("min")]
        public double? Min { get; set; }

        /// <summary>
        /// minimum value of the distribution. Also the mean value of the last bucket.
        /// </summary>
        /// <value></value>
        [JsonProperty("max")]
        public double? Max { get; set; }

        /// <summary>
        /// Array of integer indicating the number of pixels included in the bucket.
        /// </summary>
        /// <value></value>
        [JsonProperty("buckets")]
        public double[] Buckets { get; set; }
    }
}
