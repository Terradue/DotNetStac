using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Catalog;
using Stac.Extensions;
using Stac.Model;
using System.Net.Mime;

namespace Stac.Collection
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class StacCollection : StacCatalog, IStacObject, IStacCollection
    {
        public readonly static ContentType COLLECTION_MEDIATYPE = new ContentType("application/json; profile=stac-collection");
        private string license;
        private StacExtent extent;
        private Dictionary<string, IStacSummaryItem> summaries;
        private Collection<StacProvider> providers;
        private Collection<string> keywords;

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
                if (summaries == null)
                    summaries = new Dictionary<string, IStacSummaryItem>();
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

        [JsonProperty("keywords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Collection<string> Keywords
        {
            get
            {
                if (keywords == null)
                    keywords = new Collection<string>();
                return keywords;
            }
            set
            {
                keywords = value;
            }
        }

        [JsonIgnore]
        public override ContentType MediaType => CATALOG_MEDIATYPE;

    }
}
