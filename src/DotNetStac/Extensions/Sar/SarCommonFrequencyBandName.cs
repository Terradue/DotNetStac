// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SarCommonFrequencyBandName.cs

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Sar
{
    /// <summary>
    /// SAR common frequency band name
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SarCommonFrequencyBandName
    {
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        P,
        L,
        S,
        C,
        X,
        Ku,
        K,
        Ka,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1602 // EnumerationItemsMustBeDocumented
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}
