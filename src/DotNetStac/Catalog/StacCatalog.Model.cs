using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using DotNetStac;
using DotNetStac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Converters;
using Stac.Extensions;
using Stac.Model;

namespace Stac.Catalog
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class StacCatalog : IStacObject, IStacCatalogVersion, IStacCatalog, IInternalStacObject
    {
        private readonly string id;
        private Collection<StacLink> links;

        private string stacVersion = StacVersionList.Current;

        private Collection<IStacExtension> extensions;

        private string description;


        private string title;

        private Uri sourceUri;

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
        public string Id { get => id; }

        public string Title { get => title; set => title = value; }

        string IStacObject.Id => throw new NotImplementedException();

        string IStacObject.StacVersion => throw new NotImplementedException();

        Uri IStacObject.Uri => throw new NotImplementedException();

        Collection<IStacExtension> IStacObject.StacExtensions => throw new NotImplementedException();

        Collection<StacLink> IStacObject.Links => throw new NotImplementedException();

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
        }

        IStacCatalogVersion IStacCatalogVersion.Upgrade()
        {
            return this;
        }
    }
}
