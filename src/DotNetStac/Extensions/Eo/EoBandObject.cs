﻿using System.Collections.Generic;
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

        IDictionary<string, object> properties;

        /// <summary>
        /// Initialize a new Band Object
        /// </summary>
        /// <param name="name">Name of the band</param>
        /// <param name="commonName">Common name of the band</param>
        public EoBandObject(string name, EoBandCommonName? commonName)
        {
            this.name = name;
            this.commonName = commonName;
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// The name of the band (e.g., "B01", "B8", "band2", "red").
        /// </summary>
        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Description to fully explain the band. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// The name commonly used to refer to the band to make it easier to search for bands across instruments.
        /// </summary>
        /// <value></value>
        [JsonProperty("common_name")]
        public EoBandCommonName? CommonName { get => commonName; set => commonName = value; }

        /// <summary>
        /// The center wavelength of the band, in micrometers (μm).
        /// </summary>
        /// <value></value>
        [JsonProperty("center_wavelength")]
        public double? CenterWavelength { get; set; }

        /// <summary>
        /// Full width at half maximum (FWHM). The width of the band, as measured at half the maximum transmission, in micrometers (μm).
        /// </summary>
        /// <value></value>
        [JsonProperty("full_width_half_max")]
        public double? FullWidthHalfMax { get; set; }

        /// <summary>
        /// The solar illumination of the band, as measured at half the maximum transmission, in W/m2/micrometers.
        /// </summary>
        /// <value></value>
        [JsonProperty("solar_illumination")]
        public double? SolarIllumination { get; set; }

        /// <summary>
        /// Additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

    }
}
