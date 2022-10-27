using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Datacube
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatacubeAxis
    {
        x,

        y,

        z
    }
}
