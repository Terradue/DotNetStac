using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Sar
{
    /// <summary>
    /// Antenna obervation direction
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ObservationDirection
    {
        /// left
        [EnumMember(Value = "left")]
        Left,

        /// right
        [EnumMember(Value = "right")]
        Right
    }
}