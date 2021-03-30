using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Stac.Collection;
using Stac.Extensions;
using Stac.Extensions;

namespace Stac.Converters
{
    internal class StacExtensionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEnumerable<string>));
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IEnumerable<string> extensions = serializer.Deserialize<IEnumerable<string>>(reader);

            return new StacExtensions(extensions);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Dictionary<string, IStacSummaryItem> summaries = (Dictionary<string, IStacSummaryItem>)value;

            serializer.Serialize(writer, summaries.ToDictionary(k => k.Key, k => k.Value.AsJToken));
        }
    }
}