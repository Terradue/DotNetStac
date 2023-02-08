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
#pragma warning disable SA1602 // Enumeration items should be documented
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
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}
