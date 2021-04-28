using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Stac.Collection;
using Stac.Model;

namespace Stac.Extensions.ItemCollections
{
    public class ItemCollection : StacCollection, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";

        public ItemCollection(string id,
                              string description,
                              List<StacItem> stacItems) : base(id,
                                                               description,
                                                               null)
        {
            if (stacItems != null)
            {
                Features = new List<StacItem>(stacItems);
                Extent = StacExtent.Create(stacItems);
            }
        }

        /// <summary>
        /// STAC type (FeatureCollection)
        /// </summary>
        [JsonProperty("type")]
        public override string Type => "FeatureCollection";

        [JsonProperty(PropertyName = "features", Required = Required.Always)]
        public List<StacItem> Features { get; set; }

        public string Identifier => JsonSchemaUrl;

        public IDictionary<string, CreateSummary> GetSummaryFunctions()
        {
            return new Dictionary<string, CreateSummary>();
        }

    }
}
