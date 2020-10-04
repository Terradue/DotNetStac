using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Sar
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SarCommonFrequencyBandName
    {

        P,
        L,
        S,
        C,
        X,
        Ku,
        K,
        Ka
    }
}