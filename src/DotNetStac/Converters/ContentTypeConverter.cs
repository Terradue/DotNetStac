// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ContentTypeConverter.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Stac
{
    internal class ContentTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ContentType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var ct = new ContentType((string)reader.Value);
                return ct;
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidStacDataException(string.Format("Error deserializing Content Type string '{0}' : {1}", (string)reader.Value, e.Message), e);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var strs = (value as ContentType).ToString().Split(';');
            List<string> parts = new List<string>();
            parts.Add(strs[0]);
            parts.AddRange(strs.Skip(1).OrderBy(s => s));
            writer.WriteValue(string.Join(";", parts));
        }
    }
}
