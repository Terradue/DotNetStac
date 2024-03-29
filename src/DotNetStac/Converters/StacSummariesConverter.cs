﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSummariesConverter.cs

using System;
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
            return objectType == typeof(Dictionary<string, IStacSummaryItem>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<string, IStacSummaryItem> summaries = new Dictionary<string, IStacSummaryItem>();
            Dictionary<string, object> objDic = serializer.Deserialize<Dictionary<string, object>>(reader);

            foreach (var key in objDic.Keys)
            {
                if (objDic[key] is JArray)
                {
                    JArray enumerable = objDic[key] as JArray;
                    switch (enumerable.First().Type)
                    {
                        case JTokenType.Boolean:
                            summaries.Add(key, new StacSummaryValueSet<bool>(enumerable));
                            break;
                        case JTokenType.Date:
                            summaries.Add(key, new StacSummaryValueSet<DateTime>(enumerable));
                            break;
                        case JTokenType.String:
                            summaries.Add(key, new StacSummaryValueSet<string>(enumerable));
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
                    JObject obj = objDic[key] as JObject;
                    if (obj.ContainsKey("minimum") && obj.ContainsKey("maximum"))
                    {
                        switch (obj["minimum"].Type)
                        {
                            case JTokenType.Date:
                                summaries.Add(key, new StacSummaryRangeObject<DateTime>(obj));
                                break;
                            case JTokenType.String:
                                summaries.Add(key, new StacSummaryRangeObject<string>(obj));
                                break;
                            case JTokenType.Integer:
                                summaries.Add(key, new StacSummaryRangeObject<long>(obj));
                                break;
                            case JTokenType.Float:
                                summaries.Add(key, new StacSummaryRangeObject<double>(obj));
                                break;
                        }
                    }
                }
            }

            return summaries;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Dictionary<string, IStacSummaryItem> summaries = (Dictionary<string, IStacSummaryItem>)value;

            serializer.Serialize(writer, summaries.Where(k => k.Value != null).ToDictionary(k => k.Key, k => k.Value.AsJToken));
        }
    }
}
