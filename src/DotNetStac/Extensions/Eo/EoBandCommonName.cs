// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: EoBandCommonName.cs

using Newtonsoft.Json;
using Stac.Common;

namespace Stac.Extensions.Eo
{
    /// <summary>
    /// Enumeration of the allowed band common name is the name that is commonly used to refer to that band's spectral properties.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum EoBandCommonName
    {
        coastal,
        blue,
        green,
        red,
        yellow,
        pan,
        rededge,
        nir,
        nir08,
        nir09,
        cirrus,
        swir16,
        swir22,
        lwir,
        lwir11,
        lwir12,
    }
}
