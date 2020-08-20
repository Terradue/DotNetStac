using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Stac.Converters;

namespace Stac.Model.v070
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacCollection070 : StacCatalog070, IStacObject, IStacCollection, IInternalStacObject
    {
        private string license;
        private v060.StacExtent060 extent;
        private Dictionary<string, Stac.Collection.IStacSummaryItem> summaries;
        private Collection<Stac.Collection.StacProvider> providers;
        private Collection<string> keywords;

        [JsonConstructor]
        public StacCollection070(string id, string description, v060.StacExtent060 extent, IEnumerable<StacLink> links = null, string license = "proprietary") :
        base(id, description, links)
        {
            this.license = license;
            this.extent = extent;
        }

        [JsonProperty("extent")]
        public Stac.Model.v060.StacExtent060 Extent { get => extent; set => extent = value; }

        [JsonProperty("summaries")]
        [JsonConverter(typeof(StacSummariesConverter))]
        public Dictionary<string, Stac.Collection.IStacSummaryItem> Summaries
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
        public Collection<Stac.Collection.StacProvider> Providers
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

        public override IStacObject Upgrade()
        {
            var collection = new Collection.StacCollection(this.Id,
                      this.Description, this.extent.Upgrade(), this.Links);
            collection.StacExtensions = this.StacExtensions;
            collection.Title = this.Title;
            collection.Keywords = this.Keywords;
            collection.License = this.License;
            collection.Providers = this.Providers;
            collection.Summaries = this.Summaries;
            return collection;
        }
    }
}
