// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ObservationDirection.cs

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
        // left
        [EnumMember(Value = "left")]
        Left,

        // right
        [EnumMember(Value = "right")]
        Right,
    }
}
