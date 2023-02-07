using Newtonsoft.Json;

namespace Stac.Extensions.Disaster
{
    /// <summary>
    /// Activation status
    /// </summary>
    public enum DisastersActivationStatus
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("open")]
        Open,
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("closed")]
        Closed,
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("archived")]
        Archived,
    }
}
