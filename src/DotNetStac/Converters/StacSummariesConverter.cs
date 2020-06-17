using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Collection;

namespace Stac.Converters
{
    internal class StacSummariesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Dictionary<string, IStacSummaryItem>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<string, IStacSummaryItem> summaries = new Dictionary<string, IStacSummaryItem>();
            Dictionary<string, object> objDic = serializer.Deserialize<Dictionary<string, object>>(reader);

            foreach (var key in objDic.Keys)
            {
                if (objDic[key] is JArray)
                {
                    JArray enumerable = (objDic[key] as JArray);
                    switch (enumerable.First().Type)
                    {
                        case JTokenType.Boolean:
                            summaries.Add(key, new StacSummaryValueSet<bool>(enumerable));
                            break;
                        case JTokenType.Date:
                            summaries.Add(key, new StacSummaryValueSet<DateTime>(enumerable));
                            break;
                        case JTokenType.String:
                            summaries.Add(key, new StacSummaryValueSet<String>(enumerable));
                            break;
                        case JTokenType.Integer:
                            summaries.Add(key, new StacSummaryValueSet<long>(enumerable));
                            break;
                        case JTokenType.Float:
                            summaries.Add(key, new StacSummaryValueSet<double>(enumerable));
                            break;
                        case JTokenType.Object:
                            summaries.Add(key, new StacSummaryValueSet<JObject>(enumerable));
                            break;
                    }
                }
                if (objDic[key] is JObject)
                {
                    JObject obj = (objDic[key] as JObject);
                    if (obj.ContainsKey("min") && obj.ContainsKey("max"))
                    {
                        switch (obj["min"].Type)
                        {
                            case JTokenType.Date:
                                summaries.Add(key, new StacSummaryStatsObject<DateTime>(obj));
                                break;
                            case JTokenType.String:
                                summaries.Add(key, new StacSummaryStatsObject<String>(obj));
                                break;
                            case JTokenType.Integer:
                                summaries.Add(key, new StacSummaryStatsObject<long>(obj));
                                break;
                            case JTokenType.Float:
                                summaries.Add(key, new StacSummaryStatsObject<double>(obj));
                                break;
                        }
                    }
                }
            }

            return summaries;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}