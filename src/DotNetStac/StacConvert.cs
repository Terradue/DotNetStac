using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Exceptions;

namespace Stac
{
    public class StacConvert
    {
        public static T Deserialize<T>(string json) where T : IStacObject
        {
            if (typeof(T) == typeof(StacItem)
                || typeof(T) == typeof(StacCollection)
                || typeof(T) == typeof(StacCatalog))
                return JsonConvert.DeserializeObject<T>(json);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json,
            new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            Type stacType = Utils.IdentifyStacType(jobject);
            if ( typeof(T) == typeof(IStacCatalog) && !typeof(IStacCatalog).IsAssignableFrom(stacType) )
                throw new InvalidCastException(stacType + "is not IStacCatalog");

            return (T)jobject.ToObject(stacType);
        }

        public static string Serialize(IStacObject stacObject)
        {
            return JsonConvert.SerializeObject(stacObject,
            new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
        }

        public static T Deserialize<T>(Stream stream) where T : IStacObject
        {
            StreamReader sr = new StreamReader(stream);
            return Deserialize<T>(sr.ReadToEnd());
        }
    }
}
