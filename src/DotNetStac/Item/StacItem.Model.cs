using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;
using Stac.Model;
using System.Linq;
using System.Net.Mime;

namespace Stac.Item
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class StacItem : GeoJSON.Net.Feature.Feature, IStacObject, IInternalStacObject, IStacItem
    {
        public readonly static ContentType ITEM_MEDIATYPE = new ContentType("application/json; profile=stac-collection");

        private Collection<StacLink> links;

        private Dictionary<string, StacAsset> assets;

        private string stacVersion = StacVersionList.Current;

        private string collection;

        private Uri sourceUri;

        private string[] stacExtensionsStrings = new string[0];

        [JsonConstructor]
        public StacItem(IGeometryObject geometry, IDictionary<string, object> properties = null, string id = null) : base(Preconditions.CheckNotNull(geometry), properties, id)
        {
            if (geometry == null) throw new ArgumentNullException("geometry");
        }

        public StacItem(IGeometryObject geometry, object properties, string id = null) : base(Preconditions.CheckNotNull(geometry), properties, id)
        {
            if (geometry == null) throw new ArgumentNullException("geometry");
            if (properties == null) throw new ArgumentNullException("properties");
        }

        public StacItem(StacItem stacItem) : this(Preconditions.CheckNotNull(stacItem).Geometry, 
                                                  new Dictionary<string, object>(Preconditions.CheckNotNull(stacItem).Properties), 
                                                  Preconditions.CheckNotNull(stacItem).Id)
        {
            this.stacExtensionsStrings = stacItem.stacExtensionsStrings;
            this.stacVersion = stacItem.stacVersion;
            this.links = new Collection<StacLink>(stacItem.Links);
            this.assets = new Dictionary<string, StacAsset>(stacItem.Assets);
            this.collection = stacItem.collection;
            this.sourceUri = stacItem.sourceUri;
            this.extensions = new StacExtensions(stacItem.StacExtensions);
        }

        [JsonProperty("stac_extensions")]
        public string[] StacExtensionsStrings { get => stacExtensionsStrings; set => stacExtensionsStrings = value; }


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
        public IDictionary<string, StacAsset> Assets
        {
            get
            {
                if (assets == null)
                    assets = new Dictionary<string, StacAsset>();
                return assets;
            }
        }

        [JsonProperty("collection")]
        public string Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }



        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            StacExtensions = StacExtensionsFactory.Default.LoadStacExtensions(StacExtensionsStrings.ToList(), this);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (BoundingBoxes == null)
                BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();
            StacExtensionsStrings = StacExtensionsStrings.Concat(StacExtensions.Keys).Distinct().ToArray();
        }

        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

    }
}
