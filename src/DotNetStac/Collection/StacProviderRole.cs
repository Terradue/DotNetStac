using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Collection
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StacProviderRole
    {
        licensor, 
        producer,
        processor,
        host

    }
}