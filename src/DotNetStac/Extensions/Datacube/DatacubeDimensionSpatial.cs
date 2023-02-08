// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimensionSpatial.cs

using Newtonsoft.Json;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube spatial dimension
    /// </summary>
    public class DatacubeDimensionSpatial : DatacubeDimension
    {
        private DatacubeAxis? _axis;
        private object _reference_system;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeDimensionSpatial"/> class.
        /// </summary>
        public DatacubeDimensionSpatial()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets axis of the spatial dimension.
        /// </summary>
        /// <value>
        /// Axis of the spatial dimension.
        /// </value>
        [JsonProperty("axis")]
        public DatacubeAxis? Axis { get => this._axis; set => this._axis = value; }

        /// <summary>
        /// Gets or sets the spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </summary>
        /// <value>
        /// The spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </value>
        [JsonProperty("reference_system")]
        public object ReferenceSystem { get => this._reference_system; set => this._reference_system = value; }
    }
}
