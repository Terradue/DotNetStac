using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Raster
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RasterSampling
    {
        area,

        point
    }
}