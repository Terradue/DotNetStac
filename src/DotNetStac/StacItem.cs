using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Mime;
using Semver;
using System.Collections.Specialized;
using System.IO;

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
            StacExtensions = new SortedSet<string>();
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
            this.StacVersion = stacItem.StacVersion;
            this.Links = new ObservableCollection<StacLink>(stacItem.Links);
            (Links as ObservableCollection<StacLink>).CollectionChanged += LinksCollectionChanged;
            this.Assets = new Dictionary<string, StacAsset>(stacItem.Assets);
            this.Collection = stacItem.Collection;
        }

        private void LinksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldLink in e.NewItems.Cast<StacLink>())
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
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// A list of extension identifiers the Item implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_extensions")]
        public ICollection<string> StacExtensions { get; private set; }

        /// <summary>
        /// A list of references to other documents.
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public ICollection<StacLink> Links
        {
            get; private set;
        }

        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

        # endregion IStacObject

        [JsonProperty("assets")]
        public IDictionary<string, StacAsset> Assets { get; private set; }

        /// <summary>
        /// The id of the STAC Collection this Item references to
        /// </summary>
        /// <value>gets the collection id</value>
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
                asset.ParentStacObject = this;
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
