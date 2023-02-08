// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: Utils.cs

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Exceptions;

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
        /// <param name="jsonObject">JObject</param>
        /// <returns>STAC Type</returns>
        public static Type IdentifyStacType(JObject jsonObject)
        {
            if (jsonObject.Value<string>("type") == "Feature")
            {
                return typeof(StacItem);
            }
            else if (jsonObject.Value<string>("type") == "Collection" || jsonObject["extent"] != null || jsonObject["license"] != null)
            {
                return typeof(StacCollection);
            }
            else if (jsonObject.Value<string>("type") == "Catalog" || jsonObject["description"] != null)
            {
                return typeof(StacCatalog);
            }
            else if (jsonObject.ContainsKey("links") && jsonObject["links"].Type == JTokenType.Array)
            {
                return typeof(SimpleLinksCollectionObject);
            }
            else
            {
                throw new InvalidStacDataException($"{jsonObject.Value<string>("id")}. Unknown data");
            }
        }
    }
}
