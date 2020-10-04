using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;
using Stac.Model.v060;

namespace Stac.Model.v070
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class StacCatalog070 : IStacObject, IStacCatalog, IInternalStacObject
    {
        private readonly string id;
        private Collection<StacLink> links;

        private string stacVersion = StacVersionList.Current;

        private StacExtensions extensions;

        private string description;


        private string title;
        private Uri sourceUri;

        [JsonConstructor]
        public StacCatalog070(string id, string description, IEnumerable<StacLink> links = null)
        {
            this.id = id;
            this.description = description;
            if (links == null)
                this.links = new Collection<StacLink>();
            else
                this.links = new Collection<StacLink>(links.ToList());
        }

        [JsonProperty("stac_extensions")]
        public string[] StacExtensionsStrings { get; set; }

        [JsonIgnore]
        public StacExtensions StacExtensions
        {
            get
            {
                if (extensions == null)
                    extensions = new StacExtensions();
                return extensions;
            }
            set
            {
                extensions = value;
                extensions.InitStacObject(this);
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

        public Uri Uri { get => sourceUri; set => sourceUri = value; }

        [JsonProperty("properties")]
        [JsonExtensionData]
        public IDictionary<string, object> Properties => new Dictionary<string, object>();

        [JsonIgnore]
        public bool IsCatalog => true;

        public virtual IStacObject Upgrade()
        {
            var catalog = new Catalog.StacCatalog(this.Id,
                                this.Description,
                                this.Links);
            catalog.StacExtensions = this.StacExtensions;
            catalog.Title = this.Title;

            return catalog;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
        }
    }
}
