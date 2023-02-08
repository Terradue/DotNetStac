// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeVariableType.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube variable type
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatacubeVariableType
    {
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
        /// <summary>
        /// a variable indicating some measured value, for example "precipitation", "temperature", etc.
        /// </summary>
        data,

        /// <summary>
        /// a variable that contains coordinate data, but isn't a dimension in cube:dimensions. For example, the values of the datacube might be provided in the projected coordinate reference system, but the datacube could have a variable lon with dimensions (y, x), giving the longitude at each point.
        /// </summary>
        auxiliary,
#pragma warning restore SA1602 // EnumerationItemsMustBeDocumented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}
