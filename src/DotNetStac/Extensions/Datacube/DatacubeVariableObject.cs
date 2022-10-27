using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stac.Common;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/datacube#variable-object">Variable Object</seealso>
    /// of the Datacube extension
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DatacubeVariable : IStacPropertiesContainer
    {
        IDictionary<string, object> properties;
        protected string[] dimensions;
        protected DatacubeVariableType? type;
        protected string description;
        protected double[] extent;
        protected string[] values;
        private string unit;

        /// <summary>
        /// The dimensions of the variable. This should refer to keys in the cube:dimensions object or be an empty list if the variable has no dimensions.
        /// </summary>
        [JsonProperty("dimensions")]
        public string[] Dimensions { get => dimensions; set => dimensions = value; }

        /// <summary>
        /// Type of the variable.
        /// </summary>
        [JsonProperty("type")]
        public DatacubeVariableType? Type { get => type; set => type = value; }

        /// <summary>
        /// Detailed multi-line description to explain the variable. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// If the variable consists of <seealso href="https://en.wikipedia.org/wiki/Level_of_measurement#Ordinal_scale">ordinal</seealso> values, the extent (lower and upper bounds) of the values as two-element array. Use null for open intervals.
        /// </summary>
        [JsonProperty("extent")]
        public double[] Extent { get => extent; set => extent = value; }

        /// <summary>
        /// An (ordered) list of all values, especially useful for <seealso href="https://en.wikipedia.org/wiki/Level_of_measurement#Nominal_level">nominal</seealso> values.
        /// </summary>
        [JsonProperty("values")]
        public string[] Values { get => values; set => values = value; }

        /// <summary>
        /// The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get => unit; set => unit = value; }

        /// <summary>
        /// Additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

        [JsonIgnore]
        public IStacObject StacObjectContainer => null;


        public DatacubeVariable()
        {
            properties = new Dictionary<string, object>();
        }
    }
}
