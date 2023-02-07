// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SarCommonFrequencyBandName.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Sar
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SarCommonFrequencyBandName
    {
        P,
        L,
        S,
        C,
        X,
        Ku,
        K,
        Ka,
    }
}
