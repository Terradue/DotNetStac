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
using Stac.Model;
using System.Net.Mime;

namespace Stac.Catalog
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class StacCatalog : IStacObject, IStacCatalog, IInternalStacObject
    {
        public readonly static ContentType CATALOG_MEDIATYPE = new ContentType("application/json; profile=stac-catalog");

        private readonly string id;
        private Collection<StacLink> links;

        private string stacVersion = StacVersionList.Current;

        private StacExtensions extensions;

        private string description;

        private IDictionary<string, object> properties;


        private string title;

        private Uri sourceUri;
        private string[] stacExtensionsStrings = new string[0];

        [JsonIgnore]
        public Uri Uri { get => sourceUri; set => sourceUri = value; }


        [JsonConstructor]
        public StacCatalog(string id, string description, IEnumerable<StacLink> links = null)
        {
            this.id = id;
            this.description = description;
            if (links == null)
                this.links = new Collection<StacLink>();
            else
                this.links = new Collection<StacLink>(links.ToList());
            properties = new Dictionary<string, object>();
        }

        [JsonProperty("stac_extensions")]
        public string[] StacExtensionsStrings { get => stacExtensionsStrings; set => stacExtensionsStrings = value; }

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
        public string Id { get => id; }

        [JsonProperty("title")]
        public string Title { get => title; set => title = value; }

        [JsonExtensionData]
        public IDictionary<string, object> Properties
        {
            get
            {
                return properties;
            }

            set
            {
                properties = value;
            }
        }

        [JsonIgnore]
        public bool IsCatalog => true;

        [JsonIgnore]
        public virtual ContentType MediaType => CATALOG_MEDIATYPE;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            StacExtensions = StacExtensionsFactory.Default.LoadStacExtensions(StacExtensionsStrings, this);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            StacExtensionsStrings = StacExtensionsStrings.Concat(StacExtensions.Keys).Distinct().ToArray();
        }

        public IStacObject Upgrade()
        {
            return this;
        }


    }
}
