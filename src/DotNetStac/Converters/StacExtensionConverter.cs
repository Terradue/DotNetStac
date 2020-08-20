using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac.Converters
{
    internal class StacExtensionConverter : JsonConverter
    {
        private readonly IStacExtensionsFactory stacExtensionFactory;

        public StacExtensionConverter() : this(StacExtensionsFactory.Default) { }

        public StacExtensionConverter(IStacExtensionsFactory stacExtensionFactory)
        {
            this.stacExtensionFactory = stacExtensionFactory;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(IStacExtension));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Collection<IStacExtension> stacExtensions = new Collection<IStacExtension>();
            if (reader.TokenType != JsonToken.Null)
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    JToken token = JToken.Load(reader);
                    List<string> extensionPrefixes = token.ToObject<List<string>>();
                    foreach (var extensionPrefix in extensionPrefixes)
                    {
                        var stacExtension = stacExtensionFactory.CreateStacExtension(extensionPrefix, null);
                        if (stacExtension != null)
                            stacExtensions.Add(stacExtension);
                    }
                }
            }
            return stacExtensions;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Collection<IStacExtension> stacExtensions = (Collection<IStacExtension>)value;
            writer.WriteStartArray();
            foreach (var stacExtension in stacExtensions)
            {
                writer.WriteValue(stacExtension.Id);
            }
            writer.WriteEndArray();
        }
    }
}