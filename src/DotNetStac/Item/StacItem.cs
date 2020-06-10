using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNetStac;
using DotNetStac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Converters;
using Stac.Extensions;

namespace Stac.Item
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacItem : GeoJSON.Net.Feature.Feature, IStacObject
    {
        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string stacVersion = StacVersionList.Current;

        private Collection<IStacExtension> extensions;

        [JsonConstructor]
        public StacItem(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(geometry, properties, id)
        { }

        public StacItem(IGeometryObject geometry, object properties, string id = null) : base(geometry, properties, id)
        { }

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

        [JsonProperty("assets")]
        public Dictionary<string, StacAsset> Assets
        {
            get
            {
                if (assets == null)
                    assets = new Dictionary<string, StacAsset>();
                return assets;
            }
        }
    }
}
