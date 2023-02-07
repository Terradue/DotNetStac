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

        public static string Serialize(IStacObject stacObject, JsonSerializerSettings serializerSettings = null)
        {
            if (serializerSettings == null)
            {
                serializerSettings = DefaultJsonSerializerSettings;
            }

            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            return JsonConvert.SerializeObject(stacObject, serializerSettings);
        }

        public static T Deserialize<T>(Stream stream)
            where T : IStacObject
        {
            StreamReader sr = new StreamReader(stream);
            return Deserialize<T>(sr.ReadToEnd());
        }
    }
}
