// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimensionType.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube dimension type
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatacubeDimensionType
    {
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        spatial,

        temporal,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1602 // EnumerationItemsMustBeDocumented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}
