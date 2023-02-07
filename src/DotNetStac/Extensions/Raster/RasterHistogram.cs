// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: RasterHistogram.cs

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
        /// Gets or sets number of buckets of the distribution.
        /// </summary>
        /// <value></value>
        [JsonProperty("count")]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets minimum value of the distribution. Also the mean value of the first bucket.
        /// </summary>
        /// <value></value>
        [JsonProperty("min")]
        public double? Min { get; set; }

        /// <summary>
        /// Gets or sets minimum value of the distribution. Also the mean value of the last bucket.
        /// </summary>
        /// <value></value>
        [JsonProperty("max")]
        public double? Max { get; set; }

        /// <summary>
        /// Gets or sets array of integer indicating the number of pixels included in the bucket.
        /// </summary>
        /// <value></value>
        [JsonProperty("buckets")]
        public double[] Buckets { get; set; }
    }
}
