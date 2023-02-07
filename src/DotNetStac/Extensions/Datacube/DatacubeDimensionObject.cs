// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimensionObject.cs

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
        private IDictionary<string, object> properties;
        protected string type;
        protected string description;
        protected double[] extent;
        protected object values;
        protected double? step;

        /// <summary>
        /// Gets or sets type of the dimension.
        /// </summary>
        /// <value>
        /// <placeholder>Type of the dimension.</placeholder>
        /// </value>
        [JsonProperty("type")]
        public string Type { get => this.type; set => this.type = value; }

        /// <summary>
        /// Gets or sets detailed multi-line description to explain the dimension. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// <placeholder>Detailed multi-line description to explain the dimension. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.</placeholder>
        /// </value>
        [JsonProperty("description")]
        public string Description { get => this.description; set => this.description = value; }

        /// <summary>
        /// Gets or sets extent (lower and upper bounds) of the dimension as two-element array. Open intervals with null are not allowed.
        /// </summary>
        /// <value>
        /// <placeholder>Extent (lower and upper bounds) of the dimension as two-element array. Open intervals with null are not allowed.</placeholder>
        /// </value>
        [JsonProperty("extent")]
        public double[] Extent { get => this.extent; set => this.extent = value; }

        /// <summary>
        /// Gets or sets optionally, an ordered list of all values.
        /// </summary>
        /// <value>
        /// <placeholder>Optionally, an ordered list of all values.</placeholder>
        /// </value>
        [JsonProperty("values")]
        public object Values { get => this.values; set => this.values = value; }

        /// <summary>
        /// Gets or sets the space between the values. Use null for irregularly spaced steps.
        /// </summary>
        /// <value>
        /// <placeholder>The space between the values. Use null for irregularly spaced steps.</placeholder>
        /// </value>
        [JsonProperty("step")]
        public double? Step { get => this.step; set => this.step = value; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this.properties; set => this.properties = value; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IStacObject StacObjectContainer => null;

        public DatacubeDimension()
        {
            this.properties = new Dictionary<string, object>();
        }
    }

    public class DatacubeDimensionSpatial : DatacubeDimension
    {
        protected DatacubeAxis? axis;
        protected object reference_system;

        /// <summary>
        /// Gets or sets axis of the spatial dimension.
        /// </summary>
        /// <value>
        /// <placeholder>Axis of the spatial dimension.</placeholder>
        /// </value>
        [JsonProperty("axis")]
        public DatacubeAxis? Axis { get => this.axis; set => this.axis = value; }

        /// <summary>
        /// Gets or sets the spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </summary>
        /// <value>
        /// <placeholder>The spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.</placeholder>
        /// </value>
        [JsonProperty("reference_system")]
        public object ReferenceSystem { get => this.reference_system; set => this.reference_system = value; }

        public DatacubeDimensionSpatial() : base()
        {
        }
    }

    public class DatacubeDimensionSpatialHorizontal : DatacubeDimensionSpatial
    {
        public DatacubeDimensionSpatialHorizontal() : base()
        {
            this.axis = DatacubeAxis.x;
        }
    }

    public class DatacubeDimensionSpatialVertical : DatacubeDimensionSpatial
    {
        private string unit;

        /// <summary>
        /// Gets or sets the unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        
        /// </summary>
        /// <value>
        /// <placeholder>The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        </placeholder>
        /// </value>
        [JsonProperty("unit")]
        public string Unit { get => this.unit; set => this.unit = value; }

        public DatacubeDimensionSpatialVertical() : base()
        {
            this.axis = DatacubeAxis.z;
        }
    }

    public class DatacubeDimensionTemporal : DatacubeDimension
    {

        public DatacubeDimensionTemporal() : base()
        {

        }
    }

    public class DatacubeDimensionAdditional : DatacubeDimension
    {

        private string unit;
        protected object reference_system;

        /// <summary>
        /// Gets or sets the unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        
        /// </summary>
        /// <value>
        /// <placeholder>The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        </placeholder>
        /// </value>
        [JsonProperty("unit")]
        public string Unit { get => this.unit; set => this.unit = value; }

        /// <summary>
        /// Gets or sets the spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </summary>
        /// <value>
        /// <placeholder>The spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.</placeholder>
        /// </value>
        [JsonProperty("reference_system")]
        public object ReferenceSystem { get => this.reference_system; set => this.reference_system = value; }

        public DatacubeDimensionAdditional() : base()
        {

        }
    }
}
