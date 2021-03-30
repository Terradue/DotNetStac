using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stac
{
    /// <summary>
    /// Utilities static class with helper methods
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Identify the STAC object type from JSON string
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>STAC Type</returns>
        public static Type IdentifyStacType(string json)
        {
            return IdentifyStacType(JsonConvert.DeserializeObject<JObject>(json));
        }

        /// <summary>
        /// Identify the STAC object from a JObject
        /// </summary>
        /// <param name="jObject">JObject</param>
        /// <returns>STAC Type</returns>
        public static Type IdentifyStacType(JObject jObject)
        {
            if (jObject.Value<string>("type") == "Feature")
            {
                return typeof(StacItem);
            }
            else if (jObject.Value<string>("type") == "FeatureCollection")
            {
                throw new NotSupportedException($"{jObject.Value<string>("id")}. Skipping; STAC ItemCollections not supported yet");
            }
            else if (jObject.Value<string>("type") == "Collection" || jObject["extent"] == null || jObject["license"] == null)
            {
                return typeof(StacCollection);
            }
            else if (jObject.Value<string>("type") == "Catalog" || jObject["description"] == null)
            {
                return typeof(StacCatalog);
            }
            else
            {
                throw new InvalidStacDataException($"{jObject.Value<string>("id")}. Unknown data");
            }
        }
    }
}
