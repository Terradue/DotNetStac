// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacItem.cs

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
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class StacItem : GeoJSON.Net.Feature.Feature, IStacObject, ICloneable
    {
        public const string MEDIATYPE = "application/geo+json";
        public static readonly ContentType ITEM_MEDIATYPE = new ContentType(MEDIATYPE);

        private readonly StacItemRootPropertyContainer Root;

        [JsonConstructor]
        public StacItem(
            string id,
                        IGeometryObject geometry,
                        IDictionary<string, object> properties = null)
            : base(geometry, properties, id)
        {
            Preconditions.CheckNotNull(id, "id");
            this.StacExtensions = new SortedSet<string>();
            this.Root = new StacItemRootPropertyContainer(this);
            this.StacVersion = Versions.StacVersionList.Current;
            this.Links = new ObservableCollection<StacLink>();
            (this.Links as ObservableCollection<StacLink>).CollectionChanged += this.LinksCollectionChanged;
            this.Assets = new Dictionary<string, StacAsset>();

        }

        public StacItem(StacItem stacItem)
            : base(
                Preconditions.CheckNotNull(stacItem, "stacItem").Geometry,
                                                  new Dictionary<string, object>(Preconditions.CheckNotNull(stacItem).Properties),
                                                  Preconditions.CheckNotNull(stacItem, "id").Id)
        {
            this.StacExtensions = new SortedSet<string>(stacItem.StacExtensions);
            this.Root = new StacItemRootPropertyContainer(this);
            this.StacVersion = stacItem.StacVersion;
            this.Links = new ObservableCollection<StacLink>(stacItem.Links.Select(l => new StacLink(l)));
            (this.Links as ObservableCollection<StacLink>).CollectionChanged += this.LinksCollectionChanged;
            this.Assets = new Dictionary<string, StacAsset>(stacItem.Assets.Select(a => new KeyValuePair<string, StacAsset>(a.Key, new StacAsset(a.Value, this)))
                                                                           .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            this.Collection = stacItem.Collection;
            this.BoundingBoxes = stacItem.BoundingBoxes;
            this.CRS = stacItem.CRS;
        }

        /// <summary>
        /// Gets or sets the STAC version the Item implements
        /// </summary>
        /// <value>
        /// The STAC version the Item implements
        /// </value>
        [JsonProperty("stac_version", Order = -10)]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// Gets a list of extension identifiers the Item implements
        /// </summary>
        /// <value>
        /// A list of extension identifiers the Item implements
        /// </value>
        [JsonProperty("stac_extensions", Order = -9)]
        public ICollection<string> StacExtensions { get; private set; }

        /// <summary>
        /// Gets a list of references to other documents.
        /// </summary>
        /// <value>
        /// A list of references to other documents.
        /// </value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links", Order = 5)]
        public ICollection<StacLink> Links
        {
            get; private set;
        }

        /// <inheritdoc/>
        [JsonIgnore]
        public ContentType MediaType => ITEM_MEDIATYPE;

        [JsonProperty("assets", Order = 4)]
        public IDictionary<string, StacAsset> Assets { get; private set; }

        private void LinksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldLink in e.OldItems.Cast<StacLink>())
                {
                    if (oldLink.RelationshipType == "collection")
                    {
                        this.Collection = null;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newLink in e.NewItems.Cast<StacLink>())
                {
                    if (newLink.RelationshipType == "collection" && string.IsNullOrEmpty(this.Collection))
                    {
                        this.Collection = Path.GetFileNameWithoutExtension(newLink.Uri.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the id of the STAC Collection this Item references to
        /// </summary>
        /// <value>gets the collection id</value>
        [JsonProperty("__collection", Required = Required.Default)]
        [JsonIgnore]
        public string Collection
        {
            get => this.Root.GetProperty<string>("collection");
            set
            {
                if (value != null)
                {
                    this.Root.SetProperty("collection", value);
                }
                else
                {
                    this.Root.RemoveProperty("collection");
                }
            }
        }

        /// <summary>
        /// Gets item root extended data
        /// </summary>
        /// <value>
        /// Item root extended data
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> RootProperties { get => this.Root.Properties; internal set => this.Root.Properties = value; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IStacObject StacObjectContainer => this;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in this.Links)
            {
                link.Parent = this;
            }

            foreach (StacAsset asset in this.Assets.Values)
            {
                asset.ParentStacObject = this;
            }

            this.StacExtensions = new SortedSet<string>(this.StacExtensions);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (this.BoundingBoxes == null && this.Geometry != null)
            {
                this.BoundingBoxes = this.GetBoundingBoxFromGeometryExtent();
            }
        }

        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.StacExtensions.Count > 0;
        }

        /// <summary>
        /// Create a new Stac Item from this existing one
        /// </summary>
        public object Clone()
        {
            return new StacItem(this);
        }
    }
}
