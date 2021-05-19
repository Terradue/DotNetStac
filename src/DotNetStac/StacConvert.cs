using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Exceptions;
using Stac.Extensions.ItemCollections;

namespace Stac
{
    public class StacConvert
    {
        public static T Deserialize<T>(string json) where T : IStacObject
        {
            if (typeof(T) == typeof(StacItem)
                || typeof(T) == typeof(StacCollection)
                || typeof(T) == typeof(StacCatalog)
                || typeof(T) == typeof(ItemCollection))
                return JsonConvert.DeserializeObject<T>(json);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json,
            new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            Type stacType = Utils.IdentifyStacType(jobject);
            if (typeof(T) == typeof(IStacCatalog) && !typeof(IStacCatalog).IsAssignableFrom(stacType))
                throw new InvalidCastException(stacType + "is not IStacCatalog");
            try
            {
                return (T)jobject.ToObject(stacType);
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidStacDataException(string.Format("STAC object with ID '{0}' cannot be deserialized : {1}", jobject["id"], e.Message), e);
            }
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
