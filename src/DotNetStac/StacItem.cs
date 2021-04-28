using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Extensions;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using Semver;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacItem : GeoJSON.Net.Feature.Feature, IStacObject
    {
        public const string MEDIATYPE = "application/geo+json";
        public readonly static ContentType ITEM_MEDIATYPE = new ContentType(MEDIATYPE);

        [JsonConstructor]
        public StacItem(string id,
                        IGeometryObject geometry,
                        IDictionary<string, object> properties = null) :
                        base(geometry, properties, id)
        {
            Preconditions.CheckNotNull(id, "id");
            StacExtensions = new Collection<string>();
            StacVersion = Versions.StacVersionList.Current;
            Links = new Collection<StacLink>();
            Assets = new Dictionary<string, StacAsset>();
        }

        public StacItem(StacItem stacItem) : base(Preconditions.CheckNotNull(stacItem, "stacItem").Geometry,
                                                  new Dictionary<string, object>(Preconditions.CheckNotNull(stacItem).Properties),
                                                  Preconditions.CheckNotNull(stacItem, "id").Id)
        {
            this.StacExtensions = stacItem.StacExtensions;
            this.StacVersion = stacItem.StacVersion;
            this.Links = new Collection<StacLink>(stacItem.Links);
            this.Assets = new Dictionary<string, StacAsset>(stacItem.Assets);
            this.Collection = stacItem.Collection;
        }

        # region IStacObject

        /// <summary>
        /// The STAC version the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// A list of extension identifiers the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_extensions")]
        public Collection<string> StacExtensions { get; private set; }

        /// <summary>
        /// A list of references to other documents.
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public Collection<StacLink> Links
        {
            get; private set;
        }

        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

        # endregion IStacObject

        [JsonProperty("assets")]
        public IDictionary<string, StacAsset> Assets { get; private set; }

        [JsonProperty("collection")]
        public string Collection { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            foreach (StacAsset asset in Assets.Values)
            {
                asset.ParentStacItem = this;
            }
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (BoundingBoxes == null)
                BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();
        }

        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return StacExtensions.Count > 0;
        }

        [JsonIgnore]
        public IStacObject StacObjectContainer => this;
    }
}
