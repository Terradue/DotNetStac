// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeVariable.cs

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/datacube#variable-object">Variable Object</seealso>
    /// of the Datacube extension
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DatacubeVariable : IStacPropertiesContainer
    {
        protected string[] dimensions;
        private IDictionary<string, object> _properties;
        protected DatacubeVariableType? type;
        protected string description;
        protected double[] extent;
        protected string[] values;
        private string _unit;

        public DatacubeVariable()
        {
            this._properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the dimensions of the variable. This should refer to keys in the cube:dimensions object or be an empty list if the variable has no dimensions.
        /// </summary>
        /// <value>
        /// The dimensions of the variable. This should refer to keys in the cube:dimensions object or be an empty list if the variable has no dimensions.
        /// </value>
        [JsonProperty("dimensions")]
        public string[] Dimensions { get => this.dimensions; set => this.dimensions = value; }

        /// <summary>
        /// Gets or sets type of the variable.
        /// </summary>
        /// <value>
        /// Type of the variable.
        /// </value>
        [JsonProperty("type")]
        public DatacubeVariableType? Type { get => this.type; set => this.type = value; }

        /// <summary>
        /// Gets or sets detailed multi-line description to explain the variable. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// Detailed multi-line description to explain the variable. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </value>
        [JsonProperty("description")]
        public string Description { get => this.description; set => this.description = value; }

        /// <summary>
        /// Gets or sets if the variable consists of <seealso href="https://en.wikipedia.org/wiki/Level_of_measurement#Ordinal_scale">ordinal</seealso> values, the extent (lower and upper bounds) of the values as two-element array. Use null for open intervals.
        /// </summary>
        /// <value>
        /// If the variable consists of <seealso href="https://en.wikipedia.org/wiki/Level_of_measurement#Ordinal_scale">ordinal</seealso> values, the extent (lower and upper bounds) of the values as two-element array. Use null for open intervals.
        /// </value>
        [JsonProperty("extent")]
        public double[] Extent { get => this.extent; set => this.extent = value; }

        /// <summary>
        /// Gets or sets an (ordered) list of all values, especially useful for <seealso href="https://en.wikipedia.org/wiki/Level_of_measurement#Nominal_level">nominal</seealso> values.
        /// </summary>
        /// <value>
        /// An (ordered) list of all values, especially useful for <seealso href="https://en.wikipedia.org/wiki/Level_of_measurement#Nominal_level">nominal</seealso> values.
        /// </value>
        [JsonProperty("values")]
        public string[] Values { get => this.values; set => this.values = value; }

        /// <summary>
        /// Gets or sets the unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).
        /// </summary>
        /// <value>
        /// The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).
        /// </value>
        [JsonProperty("unit")]
        public string Unit { get => this._unit; set => this._unit = value; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>
        /// <value>
        /// Additional fields
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this._properties; set => this._properties = value; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IStacObject StacObjectContainer => null;
    }
}
