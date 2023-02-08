// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacConvert.cs

using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stac
{
    /// <summary>
    /// Static class with main (de)serialization methods for STAC objects.
    /// </summary>
    public static class StacConvert
    {
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Culture = CultureInfo.CreateSpecificCulture("en-US"),
        };

        /// <summary>
        /// Deserialize a STAC object from a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the STAC object to deserialize.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="serializerSettings">The JSON serializer settings to use.</param>
        /// <returns>The deserialized STAC object.</returns>
        public static T Deserialize<T>(string json, JsonSerializerSettings serializerSettings = null)
            where T : IStacObject
        {
            // if (typeof(T) == typeof(StacItem)
            //     || typeof(T) == typeof(StacCollection)
            //     || typeof(T) == typeof(StacCatalog)
            //     || typeof(T) == typeof(ItemCollection))
            //     return JsonConvert.DeserializeObject<T>(json);
            if (serializerSettings == null)
            {
                serializerSettings = DefaultJsonSerializerSettings;
            }

            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json, serializerSettings);
            Type stacType = Utils.IdentifyStacType(jobject);
            if (typeof(T) == typeof(IStacCatalog) && !typeof(IStacCatalog).IsAssignableFrom(stacType))
            {
                throw new InvalidCastException(stacType + "is not IStacCatalog");
            }

            try
            {
                // return (T)JsonConvert.DeserializeObject(json, typeof(T), serializerSettings);
                return (T)jobject.ToObject(stacType);
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidStacDataException(string.Format("STAC object with ID '{0}' cannot be deserialized : {1}", jobject["id"], e.Message), e);
            }
        }

        /// <summary>
        /// Serialize a STAC object to a JSON string.
        /// </summary>
        /// <param name="stacObject">The STAC object to serialize.</param>
        /// <param name="serializerSettings">The JSON serializer settings to use.</param>
        /// <returns>The serialized JSON string.</returns>
        public static string Serialize(IStacObject stacObject, JsonSerializerSettings serializerSettings = null)
        {
            if (serializerSettings == null)
            {
                serializerSettings = DefaultJsonSerializerSettings;
            }

            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            return JsonConvert.SerializeObject(stacObject, serializerSettings);
        }

        /// <summary>
        /// Deserialize a STAC object from a JSON stream.
        /// </summary>
        /// <typeparam name="T">The type of the STAC object to deserialize.</typeparam>
        /// <param name="stream">The JSON stream to deserialize.</param>
        /// <returns>The deserialized STAC object.</returns>
        public static T Deserialize<T>(Stream stream)
            where T : IStacObject
        {
            StreamReader sr = new StreamReader(stream);
            return Deserialize<T>(sr.ReadToEnd());
        }
    }
}
