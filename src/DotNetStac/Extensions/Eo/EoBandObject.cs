// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: EoBandObject.cs

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac.Extensions.Eo
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/eo/#band-object">Band Object</seealso>
    /// of the EO extensions
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EoBandObject
    {
        private string name;

        private string description;

        private EoBandCommonName? commonName;
        private IDictionary<string, object> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="EoBandObject"/> class.
        /// </summary>
        /// <param name="name">Name of the band</param>
        /// <param name="commonName">Common name of the band</param>
        public EoBandObject(string name, EoBandCommonName? commonName)
        {
            this.name = name;
            this.commonName = commonName;
            this.properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the name of the band (e.g., "B01", "B8", "band2", "red").
        /// </summary>
        /// <value>
        /// The name of the band (e.g., "B01", "B8", "band2", "red").
        /// </value>
        [JsonProperty("name")]
        public string Name { get => this.name; set => this.name = value; }

        /// <summary>
        /// Gets or sets description to fully explain the band. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// Description to fully explain the band. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </value>
        [JsonProperty("description")]
        public string Description { get => this.description; set => this.description = value; }

        /// <summary>
        /// Gets or sets the name commonly used to refer to the band to make it easier to search for bands across instruments.
        /// </summary>
        /// <value>
        /// The name commonly used to refer to the band to make it easier to search for bands across instruments.
        /// </value>
        [JsonProperty("common_name")]
        public EoBandCommonName? CommonName { get => this.commonName; set => this.commonName = value; }

        /// <summary>
        /// Gets or sets the center wavelength of the band, in micrometers (μm).
        /// </summary>
        /// <value>
        /// The center wavelength of the band, in micrometers (μm).
        /// </value>
        [JsonProperty("center_wavelength")]
        public double? CenterWavelength { get; set; }

        /// <summary>
        /// Gets or sets full width at half maximum (FWHM). The width of the band, as measured at half the maximum transmission, in micrometers (μm).
        /// </summary>
        /// <value>
        /// Full width at half maximum (FWHM). The width of the band, as measured at half the maximum transmission, in micrometers (μm).
        /// </value>
        [JsonProperty("full_width_half_max")]
        public double? FullWidthHalfMax { get; set; }

        /// <summary>
        /// Gets or sets the solar illumination of the band, as measured at half the maximum transmission, in W/m2/micrometers.
        /// </summary>
        /// <value>
        /// The solar illumination of the band, as measured at half the maximum transmission, in W/m2/micrometers.
        /// </value>
        [JsonProperty("solar_illumination")]
        public double? SolarIllumination { get; set; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>
        /// <value>
        /// Additional fields
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this.properties; set => this.properties = value; }

    }
}
