// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeAxis.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube axis
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatacubeAxis
    {
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        x,

        y,

        z,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1602 // EnumerationItemsMustBeDocumented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}
