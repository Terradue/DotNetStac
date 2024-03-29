﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DataType.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Common
{
    /// <summary>
    /// The data type gives information about the values
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType
    {
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        // 8-bit integer
        int8,

        // 16-bit integer
        int16,

        // 32-bit integer
        int32,

        // 64-bit integer
        int64,

        // unsigned 8-bit integer (common for 8-bit RGB PNG's)
        uint8,

        // unsigned 16-bit integer
        uint16,

        // unsigned 32-bit integer
        uint32,

        // unsigned 64-bit integer
        uint64,

        // 16-bit float
        float16,

        // 32-bit float
        float32,

        // 64-big float
        float64,

        // 16-bit complex integer
        cint16,

        // 32-bit complex integer
        cint32,

        // 32-bit complex float
        cfloat32,

        // 64-bit complex float
        cfloat64,

        // Other data type than the ones listed above (e.g. boolean, string, higher precision numbers)
        other,

#pragma warning restore SA1300 // Element should begin with upper-case letter
#pragma warning restore SA1602 // EnumerationItemsMustBeDocumented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
