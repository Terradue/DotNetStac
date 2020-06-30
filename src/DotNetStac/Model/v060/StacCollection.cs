using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DotNetStac;
using DotNetStac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Catalog;
using Stac.Converters;
using Stac.Extensions;

namespace Stac.Model.v060
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacCollection060 : StacCatalog060, IStacObject, IStacCollectionVersion, IStacCollection
    {
        private string license;
        private StacExtent060 extent;
        private Collection<Stac.Collection.StacProvider> providers;
        private Collection<string> keywords;

        [JsonConstructor]
        public StacCollection060(string id, string description, StacExtent060 extent, IEnumerable<StacLink> links = null, string license = "proprietary") :
        base(id, description, links)
        {
            this.license = license;
            this.extent = extent;
        }

        [JsonProperty("extent")]
        public StacExtent060 Extent { get => extent; set => extent = value; }

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

        [JsonExtensionData]
        public Dictionary<string, JToken> Properties { get; set; }

        IStacCollectionVersion IStacCollectionVersion.Upgrade()
        {
            var collection = new v070.StacCollection070(this.Id,
                                           this.Description,
                                           this.Extent,
                                           this.Links);
            collection.StacExtensions = this.StacExtensions;
            collection.Title = this.Title;
            collection.Keywords = this.Keywords;
            collection.License = this.License;
            collection.Providers = this.Providers;
            return collection;
        }
    }
}
