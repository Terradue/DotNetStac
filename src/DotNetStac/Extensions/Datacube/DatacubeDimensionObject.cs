using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stac.Common;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/datacube#dimension-object">Dimension Object</seealso>
    /// of the Datacube extension
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DatacubeDimension : IStacPropertiesContainer
    {
        IDictionary<string, object> properties;
        protected string type;
        protected string description;
        protected double[] extent;
        protected object values;
        protected double? step;

        /// <summary>
        /// Type of the dimension.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get => type; set => type = value; }

        /// <summary>
        /// Detailed multi-line description to explain the dimension. <seealso href="http://commonmark.org/">CommonMark 0.29</seealso> syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// Extent (lower and upper bounds) of the dimension as two-element array. Open intervals with null are not allowed.
        /// </summary>
        [JsonProperty("extent")]
        public double[] Extent { get => extent; set => extent = value; }

        /// <summary>
        /// Optionally, an ordered list of all values.
        /// </summary>
        [JsonProperty("values")]
        public object Values { get => values; set => values = value; }

        /// <summary>
        /// The space between the values. Use null for irregularly spaced steps.
        /// </summary>
        [JsonProperty("step")]
        public double? Step { get => step; set => step = value; }

        /// <summary>
        /// Additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

        [JsonIgnore]
        public IStacObject StacObjectContainer => null;


        public DatacubeDimension()
        {
            properties = new Dictionary<string, object>();
        }
    }

    public class DatacubeDimensionSpatial : DatacubeDimension
    {
        protected DatacubeAxis? axis;
        protected object reference_system;

        /// <summary>
        /// Axis of the spatial dimension.
        /// </summary>
        [JsonProperty("axis")]
        public DatacubeAxis? Axis { get => axis; set => axis = value; }

        /// <summary>
        /// The spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </summary>
        [JsonProperty("reference_system")]
        public object ReferenceSystem { get => reference_system; set => reference_system = value; }

        public DatacubeDimensionSpatial() : base()
        {
        }
    }

    public class DatacubeDimensionSpatialHorizontal : DatacubeDimensionSpatial
    {
        public DatacubeDimensionSpatialHorizontal() : base()
        {
            base.axis = DatacubeAxis.x;
        }
    }

    public class DatacubeDimensionSpatialVertical : DatacubeDimensionSpatial
    {
        private string unit;

        /// <summary>
        /// The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get => unit; set => unit = value; }

        public DatacubeDimensionSpatialVertical() : base()
        {
            base.axis = DatacubeAxis.z;
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
        /// The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).        
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get => unit; set => unit = value; }

        /// <summary>
        /// The spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </summary>
        [JsonProperty("reference_system")]
        public object ReferenceSystem { get => reference_system; set => reference_system = value; }

        public DatacubeDimensionAdditional() : base()
        {

        }
    }
}
