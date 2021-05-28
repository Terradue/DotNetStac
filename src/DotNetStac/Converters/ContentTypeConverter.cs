using System;
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
                return new ContentType((string)reader.Value);
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
