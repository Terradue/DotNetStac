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

namespace Stac.Collection
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacCollection : IStacObject
    {
        private readonly string id;
        private Collection<StacLink> links;

        private string stacVersion = StacVersionList.Current;

        private Collection<IStacExtension> extensions;

        private string description;
        private readonly string license;
        private StacExtent extent;
        private Dictionary<string, IStacSummaryItem> summaries;

        [JsonConstructor]
        public StacCollection(string id, string description, StacExtent extent, IEnumerable<StacLink> links, string license = "proprietary")
        {
            this.id = id;
            this.description = description;
            this.license = license;
            this.extent = extent;
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

        public string Id => id;
    }
}
