// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: Statistics.cs

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac.Common
{
    /// <summary>
    /// Statistics Object
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Statistics
    {
        private IDictionary<string, object> properties;

        /// <summary>
        /// Initialize a new statistics object
        /// </summary>
        /// <param name="minimum">minimum value</param>
        /// <param name="maximum">maximum value</param>
        /// <param name="mean">mean value</param>
        /// <param name="stdev">standard deviation</param>
        /// <param name="validPercent">valid percentage</param>
        [JsonConstructor]
        public Statistics(double? minimum, double? maximum, double? mean, double? stdev, double? validPercent)
        {
            this.Mean = mean;
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.Stdev = stdev;
            this.ValidPercent = validPercent;
            this.properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets mean value
        /// </summary>

        [JsonProperty("mean")]
        public double? Mean { get; set; }

        /// <summary>
        /// Gets or sets minimum value
        /// </summary>

        [JsonProperty("minimum")]
        public double? Minimum { get; set; }

        /// <summary>
        /// Gets or sets maximum value
        /// </summary>

        [JsonProperty("maximum")]
        public double? Maximum { get; set; }

        /// <summary>
        /// Gets or sets standard Deviation
        /// </summary>

        [JsonProperty("stdev")]
        public double? Stdev { get; set; }

        /// <summary>
        /// Gets or sets valid percentage
        /// </summary>

        [JsonProperty("valid_percent")]
        public double? ValidPercent { get; set; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>

        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this.properties; set => this.properties = value; }

    }
}
