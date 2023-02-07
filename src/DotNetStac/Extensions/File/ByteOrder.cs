// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ByteOrder.cs

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.File
{
    /// <summary>
    /// The byte order
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ByteOrder
    {
        /// Big Endian
        [EnumMember(Value = "big-endian")]
        BigEndian,

        /// Little Endian
        [EnumMember(Value = "little-endian")]
        LittleEndian

    }
}
