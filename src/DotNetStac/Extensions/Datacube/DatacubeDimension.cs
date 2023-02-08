// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimension.cs

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/datacube#dimension-object">Dimension Object</seealso>
    /// of the Datacube extension
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DatacubeDimension : IStacPropertiesContainer
    {
        private string _type;
        private string _description;
        private double[] _extent;
        private object _values;
        private double? _step;
        private IDictionary<string, object> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeDimension"/> class.
        /// </summary>
        public DatacubeDimension()
        {
            this._properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets type of the dimension.
        /// </summary>
        /// <value>
        /// Type of the dimension.
        /// </value>
        [JsonProperty("type")]
        public string Type { get => this._type; set => this._type = value; }

        /// <summary>
        /// Gets or sets detailed multi-line description to explain the dimension. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// Detailed multi-line description to explain the dimension. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </value>
        [JsonProperty("description")]
        public string Description { get => this._description; set => this._description = value; }

        /// <summary>
        /// Gets or sets extent (lower and upper bounds) of the dimension as two-element array. Open intervals with null are not allowed.
        /// </summary>
        /// <value>
        /// Extent (lower and upper bounds) of the dimension as two-element array. Open intervals with null are not allowed.
        /// </value>
        [JsonProperty("extent")]
        public double[] Extent { get => this._extent; set => this._extent = value; }

        /// <summary>
        /// Gets or sets optionally, an ordered list of all values.
        /// </summary>
        /// <value>
        /// Optionally, an ordered list of all values.
        /// </value>
        [JsonProperty("values")]
        public object Values { get => this._values; set => this._values = value; }

        /// <summary>
        /// Gets or sets the space between the values. Use null for irregularly spaced steps.
        /// </summary>
        /// <value>
        /// The space between the values. Use null for irregularly spaced steps.
        /// </value>
        [JsonProperty("step")]
        public double? Step { get => this._step; set => this._step = value; }

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
