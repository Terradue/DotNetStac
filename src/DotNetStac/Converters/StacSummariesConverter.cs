using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    {
                        switch (enumerable.First().Type)
                        {
                            case JTokenType.Boolean:
                                StacSummaryValueSet<bool> boolset = new StacSummaryValueSet<bool>(serializer.Deserialize<List<bool>>(enumerable.CreateReader()));
                                summaries.Add(key, boolset);
                                break;
                            case JTokenType.Date:
                                StacSummaryValueSet<DateTime> dateset = new StacSummaryValueSet<DateTime>(serializer.Deserialize<List<DateTime>>(enumerable.CreateReader()));
                                summaries.Add(key, dateset);
                                break;
                            case JTokenType.String:
                                StacSummaryValueSet<String> stringset = new StacSummaryValueSet<String>(serializer.Deserialize<List<String>>(enumerable.CreateReader()));
                                summaries.Add(key, stringset);
                                break;
                            case JTokenType.Integer:
                                StacSummaryValueSet<long> intset = new StacSummaryValueSet<long>(serializer.Deserialize<List<long>>(enumerable.CreateReader()));
                                summaries.Add(key, intset);
                                break;
                            case JTokenType.Float:
                                StacSummaryValueSet<double> floatset = new StacSummaryValueSet<double>(serializer.Deserialize<List<double>>(enumerable.CreateReader()));
                                summaries.Add(key, floatset);
                                break;
                            case JTokenType.Object:
                                StacSummaryValueSet<Dictionary<string, object>> objset = new StacSummaryValueSet<Dictionary<string, object>>(serializer.Deserialize<List<Dictionary<string, object>>>(enumerable.CreateReader()));
                                summaries.Add(key, objset);
                                break;
                        }

                    }
                }
                if (objDic[key] is JObject)
                {
                    JObject obj = (objDic[key] as JObject);
                    if ( obj.ContainsKey("min") && obj.ContainsKey("max") ){
                        switch(obj["min"].Type){
                            case JTokenType.Date:
                                StacSummaryStatsObject<DateTime> datestat = serializer.Deserialize<StacSummaryStatsObject<DateTime>>(obj.CreateReader());
                                summaries.Add(key, datestat);
                                break;
                            case JTokenType.String:
                                StacSummaryStatsObject<string> stringstat = serializer.Deserialize<StacSummaryStatsObject<string>>(obj.CreateReader());
                                summaries.Add(key, stringstat);
                                break;
                            case JTokenType.Integer:
                                StacSummaryStatsObject<long> intstat = serializer.Deserialize<StacSummaryStatsObject<long>>(obj.CreateReader());
                                summaries.Add(key, intstat);
                                break;
                            case JTokenType.Float:
                                StacSummaryStatsObject<double> doublestat = serializer.Deserialize<StacSummaryStatsObject<double>>(obj.CreateReader());
                                summaries.Add(key, doublestat);
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