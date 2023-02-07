// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeAxis.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Datacube
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatacubeAxis
    {
        x,

        y,

        z,
    }
}
