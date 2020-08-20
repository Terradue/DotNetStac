using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetStac.Converters
{
    public class CollectionConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Collection<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return new Collection<T>(token.ToObject<List<T>>());
            }
            return new Collection<T>();
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;

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
}
