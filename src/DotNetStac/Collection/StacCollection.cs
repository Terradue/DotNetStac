using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DotNetStac;
using DotNetStac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Catalog;
using Stac.Converters;
using Stac.Extensions;

namespace Stac.Collection
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacCollection : StacCatalog, IStacObject
    {
        private string license;
        private StacExtent extent;
        private Dictionary<string, IStacSummaryItem> summaries;

        private Collection<StacProvider> providers;

        [JsonConstructor]
        public StacCollection(string id, string description, StacExtent extent, IEnumerable<StacLink> links = null, string license = "proprietary") :
        base(id, description, links)
        {
            this.license = license;
            this.extent = extent;
        }

        [JsonProperty("extent")]
        public StacExtent Extent { get => extent; set => extent = value; }

        [JsonProperty("summaries")]
        [JsonConverter(typeof(StacSummariesConverter))]
        public Dictionary<string, IStacSummaryItem> Summaries
        {
            get
            {
                return summaries;
            }
            set
            {
                summaries = value;
            }
        }

        [JsonProperty("license")]
        public string License { get => license; set => license = value; }

        [JsonProperty("providers")]
        public Collection<StacProvider> Providers
        {
            get { return providers; }
            set
            {
                providers = value;
            }
        }
    }
}
