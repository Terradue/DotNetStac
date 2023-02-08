// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DisastersActivationStatus.cs

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
