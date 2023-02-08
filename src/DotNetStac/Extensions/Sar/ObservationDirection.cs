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
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        [EnumMember(Value = "left")]
        Left,

        [EnumMember(Value = "right")]
        Right,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
