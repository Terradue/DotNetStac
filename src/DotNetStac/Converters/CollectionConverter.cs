// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: CollectionConverter.cs

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stac.Converters
{
#pragma warning disable SA1649 // File name should match first type name
    /// <summary>
    /// Converter for Collection
    /// </summary>
    /// <typeparam name="T">Type of the collection</typeparam>
    public class CollectionConverter<T> : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Collection<T>);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return new Collection<T>(token.ToObject<List<T>>());
            }

            return new Collection<T>();
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Collection<T> collection = (Collection<T>)value;
            writer.WriteStartArray();
            foreach (var item in collection)
            {
                serializer.Serialize(writer, item);
            }

            writer.WriteEndArray();
        }
    }
#pragma warning restore SA1649 // File name should match first type name
}
