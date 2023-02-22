// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: Enum.cs

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Stac.Test.Common
{
    public enum Enum1
    {
        test,

        summary
    }

    public enum Enum2
    {
        [JsonProperty("test")]
        Test,

        [JsonProperty("summary")]
        Summary
    }

    public enum Enum3
    {
        [EnumMember(Value = @"cql2-text")]
        Cql2Text,

        [EnumMember(Value = @"cql2-json")]
        Cql2Json
    }
}
