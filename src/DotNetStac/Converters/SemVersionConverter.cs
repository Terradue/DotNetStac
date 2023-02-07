// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SemVersionConverter.cs

using System;
using Newtonsoft.Json;
using Semver;

namespace Stac
{
    internal class SemVersionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(SemVersion));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return SemVersion.Parse((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((value as SemVersion).ToString());
        }
    }
}
