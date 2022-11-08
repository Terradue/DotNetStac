using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Semver;
using Stac.Converters;

namespace Stac
{
    /// <summary>
    /// STAC Item Object implementing STAC Item spec (https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md)
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    public partial class StacItem : GeoJSON.Net.Feature.Feature, IStacObject, ICloneable
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
            StacExtensions = new SortedSet<string>();
            Root = new StacItemRootPropertyContainer(this);
            StacVersion = Versions.StacVersionList.Current;
            Links = new ObservableCollection<StacLink>();
            (Links as ObservableCollection<StacLink>).CollectionChanged += LinksCollectionChanged;
            Assets = new Dictionary<string, StacAsset>();

        }

        public StacItem(StacItem stacItem) : base(Preconditions.CheckNotNull(stacItem, "stacItem").Geometry,
                                                  new Dictionary<string, object>(Preconditions.CheckNotNull(stacItem).Properties),
                                                  Preconditions.CheckNotNull(stacItem, "id").Id)
        {
            this.StacExtensions = new SortedSet<string>(stacItem.StacExtensions);
            this.Root = new StacItemRootPropertyContainer(this);
            this.StacVersion = stacItem.StacVersion;
            this.Links = new ObservableCollection<StacLink>(stacItem.Links.Select(l => new StacLink(l)));
            (Links as ObservableCollection<StacLink>).CollectionChanged += LinksCollectionChanged;
            this.Assets = new Dictionary<string, StacAsset>(stacItem.Assets.Select(a => new KeyValuePair<string, StacAsset>(a.Key, new StacAsset(a.Value, this)))
                                                                           .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            this.Collection = stacItem.Collection;
            this.BoundingBoxes = stacItem.BoundingBoxes;
            this.CRS = stacItem.CRS;
        }

        private void LinksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldLink in e.OldItems.Cast<StacLink>())
                {
                    if (oldLink.RelationshipType == "collection")
                    {
                        Collection = null;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (var newLink in e.NewItems.Cast<StacLink>())
                {
                    if (newLink.RelationshipType == "collection" && string.IsNullOrEmpty(Collection))
                    {
                        Collection = Path.GetFileNameWithoutExtension(newLink.Uri.ToString());
                    }
                }
            }
        }

        #region IStacObject

        /// <summary>
        /// The STAC version the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_version", Order = -10)]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// A list of extension identifiers the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_extensions", Order = -9)]
        public ICollection<string> StacExtensions { get; private set; }

        /// <summary>
        /// A list of references to other documents.
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links", Order = 5)]
        public ICollection<StacLink> Links
        {
            get; private set;
        }

        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

        # endregion IStacObject

        [JsonProperty("assets", Order = 4)]
        public IDictionary<string, StacAsset> Assets { get; private set; }

        /// <summary>
        /// The id of the STAC Collection this Item references to
        /// </summary>
        /// <value>gets the collection id</value>
        [JsonProperty("__collection", Required = Required.Default)]
        [JsonIgnore]
        public string Collection
        {
            get => Root.GetProperty<string>("collection");
            set
            {
                if (value != null) Root.SetProperty("collection", value);
                else Root.RemoveProperty("collection");
            }
        }

        /// <summary>
        /// Item root extended data
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> RootProperties { get => Root.Properties; internal set => Root.Properties = value; }

        private StacItemRootPropertyContainer Root;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            foreach (StacAsset asset in Assets.Values)
            {
                asset.ParentStacObject = this;
            }
            StacExtensions = new SortedSet<string>(StacExtensions);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (BoundingBoxes == null && Geometry != null)
                BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();

        }

        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return StacExtensions.Count > 0;
        }

        /// <summary>
        /// Create a new Stac Item from this existing one
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new StacItem(this);
        }

        [JsonIgnore]
        public IStacObject StacObjectContainer => this;
    }
}
