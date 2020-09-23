using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Stac.Extensions.Eo
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EoBandObject
    {
        private string name;
        private EoBandCommonName commonName;

        IDictionary<string, object> properties;

        public EoBandObject(string name, EoBandCommonName commonName)
        {
            this.name = name;
            this.commonName = commonName;
            properties = new Dictionary<string, object>();
        }

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        [JsonProperty("common_name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EoBandCommonName CommonName { get => commonName; set => commonName = value; }

        [JsonProperty("center_wavelength")]
        public double CenterWavelength { get; set; }

         [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

    }
}