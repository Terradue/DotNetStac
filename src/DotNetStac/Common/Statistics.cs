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
        [JsonConstructor]
        public Statistics(double? minimum, double? maximum, double? mean, double? stdev, double? validPercent)
        {
            Mean = mean;
            Minimum = minimum;
            Maximum = maximum;
            Stdev = stdev;
            ValidPercent = validPercent;
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// mean value
        /// </summary>
        /// <value></value>
        [JsonProperty("mean")]
        public double? Mean { get; set; }

        /// <summary>
        /// minimum value
        /// </summary>
        /// <value></value>
        [JsonProperty("minimum")]
        public double? Minimum { get; set; }

        /// <summary>
        /// maximum value
        /// </summary>
        /// <value></value>
        [JsonProperty("maximum")]
        public double? Maximum { get; set; }

        /// <summary>
        /// Standard Deviation
        /// </summary>
        /// <value></value>
        [JsonProperty("stdev")]
        public double? Stdev { get; set; }

        /// <summary>
        /// valid percentage
        /// </summary>
        /// <value></value>
        [JsonProperty("valid_percent")]
        public double? ValidPercent { get; set; }

        /// <summary>
        /// Additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

    }
}
