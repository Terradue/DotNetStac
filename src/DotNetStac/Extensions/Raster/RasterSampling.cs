// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: RasterSampling.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Raster
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RasterSampling
    {
        area,

        point,
    }
}
