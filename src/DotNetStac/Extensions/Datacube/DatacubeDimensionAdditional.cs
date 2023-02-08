// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimensionAdditional.cs

using Newtonsoft.Json;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube additional dimension
    /// </summary>
    public class DatacubeDimensionAdditional : DatacubeDimension
    {
        private object _reference_system;
        private string _unit;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeDimensionAdditional"/> class.
        /// </summary>
        public DatacubeDimensionAdditional()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get => this._unit; set => this._unit = value; }

        /// <summary>
        /// Gets or sets the spatial reference system for the data, specified as <seealso href="http://www.epsg-registry.org/">numerical EPSG code</seealso>, <seealso href="http://docs.opengeospatial.org/is/18-010r7/18-010r7.html">WKT2 (ISO 19162) string</seealso> or <seealso href="https://proj.org/specifications/projjson.html">PROJJSON object</seealso>. Defaults to EPSG code 4326.
        /// </summary>
        [JsonProperty("reference_system")]
        public object ReferenceSystem { get => this._reference_system; set => this._reference_system = value; }
    }
}
