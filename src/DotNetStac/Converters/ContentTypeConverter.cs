using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Stac
{
    internal class ContentTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ContentType));
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
            writer.WriteValue((value as ContentType).ToString());
        }
    }
}
