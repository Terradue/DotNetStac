using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DotNetStac;
using DotNetStac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Converters;
using Stac.Extensions;

namespace Stac.Model.v060
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacCatalog060 : IStacObject, IStacCatalogModelVersion
    {
        private readonly string id;
        private Collection<StacLink> links;

        private string stacVersion = StacVersionList.Current;

        private Collection<IStacExtension> extensions;

        private string description;
        

        private string title;

        [JsonConstructor]
        public StacCatalog060(string id, string description, IEnumerable<StacLink> links = null)
        {
            this.id = id;
            this.description = description;
            if ( links == null )
                this.links = new Collection<StacLink>();
            else
                this.links = new Collection<StacLink>(links.ToList());
        }

        [JsonProperty("stac_extensions")]
        [JsonConverter(typeof(StacExtensionConverter))]
        public Collection<IStacExtension> StacExtensions
        {
            get
            {
                if (extensions == null)
                    extensions = new Collection<IStacExtension>();
                return extensions;
            }
            set
            {
                extensions = value;
            }
        }

        [JsonProperty("stac_version")]
        public string StacVersion
        {
            get
            {
                return stacVersion;
            }

            set
            {
                stacVersion = value;
            }
        }

        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public Collection<StacLink> Links
        {
            get
            {
                if (links == null)
                    links = new Collection<StacLink>();
                return links;
            }
            set
            {
                links = value;
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        [JsonProperty("id")]
        public string Id => id;

        public string Title { get => title; set => title = value; }

        public IStacCatalogModelVersion Upgrade()
        {
            var catalog = new v070.StacCatalog070(this.Id,
                                            this.Description,
                                            this.Links);
            catalog.StacExtensions = this.StacExtensions;
            catalog.Title = this.Title;
            return catalog;
        }
    }
}
