// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimensionSpatialVertical.cs

using Newtonsoft.Json;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube vertical spatial dimension
    /// </summary>
    public class DatacubeDimensionSpatialVertical : DatacubeDimensionSpatial
    {
        private string _unit;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeDimensionSpatialVertical"/> class.
        /// </summary>
        public DatacubeDimensionSpatialVertical()
            : base()
        {
            this.Axis = DatacubeAxis.z;
        }

        /// <summary>
        /// Gets or sets the unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).
        /// </summary>
        /// <value>
        /// The unit of measurement for the data, preferably compliant to <seealso href="https://ncics.org/portfolio/other-resources/udunits2">UDUNITS-2</seealso> units (singular).
        /// </value>
        [JsonProperty("unit")]
        public string Unit { get => this._unit; set => this._unit = value; }
    }
}
