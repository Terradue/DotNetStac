using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Exceptions;

namespace Stac
{
    public class StacConvert
    {
        public static T Deserialize<T>(string json) where T : IStacObject
        {
            if ( typeof(T) == typeof(StacItem) || typeof(T) == typeof(StacCollection) || typeof(T) == typeof(StacCatalog) )
                return JsonConvert.DeserializeObject<T>(json);
            JObject jobject = JsonConvert.DeserializeObject<JObject>(json);
            Type stacType = Utils.IdentifyStacType(jobject);
            return (T)jobject.ToObject(stacType);
        }

        public static string Serialize(IStacObject stacObject)
        {
            return JsonConvert.SerializeObject(stacObject);
        }
    }
}
