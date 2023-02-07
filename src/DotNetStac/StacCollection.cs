// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacCollection.cs

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Semver;
using Stac.Collection;
using Stac.Converters;
using Stac.Extensions;

namespace Stac
{
    /// <summary>
    /// STAC Collection Object implementing STAC Collection spec (https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md)
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    public partial class StacCollection : IStacObject, IStacParent, IStacCatalog, ICloneable
    {
        public const string MEDIATYPE = "application/json";
        public static readonly ContentType COLLECTION_MEDIATYPE = new ContentType(MEDIATYPE);

        [JsonConstructor]
        public StacCollection(
            string id,
            string description,
            StacExtent extent,
            IDictionary<string, StacAsset> assets = null,
            IEnumerable<StacLink> links = null,
            string license = "proprietary")
        {
            this.Properties = new Dictionary<string, object>();
            this.Id = id;
            this.StacVersion = Versions.StacVersionList.Current;
            this.Description = description;
            if (links == null)
            {
                this.Links = new Collection<StacLink>();
            }
            else
            {
                this.Links = new Collection<StacLink>(links.ToList());
            }

            if (assets == null)
            {
                this.Assets = new Dictionary<string, StacAsset>();
            }
            else
            {
                this.Assets = new Dictionary<string, StacAsset>(assets);
            }

            this.Summaries = new Dictionary<string, IStacSummaryItem>();
            this.StacExtensions = new SortedSet<string>();
            this.License = license;
            this.Keywords = new Collection<string>();
            this.Extent = extent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacCollection"/> class.
        /// </summary>
        /// <param name="stacCollection">existing Stac Collection</param>
        public StacCollection(StacCollection stacCollection)
        {
            this.Id = stacCollection.Id;
            this.StacExtensions = new SortedSet<string>(stacCollection.StacExtensions);
            this.StacVersion = stacCollection.StacVersion;
            this.Links = new Collection<StacLink>(stacCollection.Links.ToList());
            this.Summaries = new Dictionary<string, IStacSummaryItem>(stacCollection.Summaries);
            this.Properties = new Dictionary<string, object>(stacCollection.Properties);
            this.Assets = new Dictionary<string, StacAsset>(stacCollection.Assets.Select(a => new KeyValuePair<string, StacAsset>(a.Key, new StacAsset(a.Value, this)))
                                                                           .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            this.License = stacCollection.License;
            this.Keywords = new Collection<string>(stacCollection.Keywords);
            this.Extent = new StacExtent(stacCollection.Extent);
        }

        /// <summary>
        /// Gets identifier for the Collection.
        /// </summary>
        /// <value>
        /// Identifier for the Collection.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets or sets the STAC version the Collection implements
        /// </summary>
        /// <value>
        /// The STAC version the Collection implements
        /// </value>
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// Gets a list of extension identifiers the Collection implements
        /// </summary>
        /// <value>
        /// A list of extension identifiers the Collection implements
        /// </value>
        [JsonProperty("stac_extensions")]
        public ICollection<string> StacExtensions { get; private set; }

        /// <summary>
        /// Gets a list of references to other documents.
        /// </summary>
        /// <value>
        /// A list of references to other documents.
        /// </value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public ICollection<StacLink> Links
        {
            get; internal set;
        }

        /// <inheritdoc/>
        [JsonIgnore]
        public ContentType MediaType => COLLECTION_MEDIATYPE;

        /// <summary>
        /// Gets sTAC type (Collection)
        /// </summary>
        /// <value>
        /// STAC type (Collection)
        /// </value>
        [JsonProperty("type")]
        public virtual string Type => "Collection";

        /// <summary>
        /// Gets a map of property summaries, either a set of values or statistics such as a range.
        /// </summary>
        /// <value>
        /// A map of property summaries, either a set of values or statistics such as a range.
        /// </value>
        [JsonProperty("summaries")]
        [JsonConverter(typeof(StacSummariesConverter))]
        public Dictionary<string, IStacSummaryItem> Summaries { get; internal set; }

        /// <summary>
        /// Gets collection extended data
        /// </summary>
        /// <value>
        /// Collection extended data
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; internal set; }

        /// <summary>
        /// Gets spatial and temporal extents.
        /// </summary>
        /// <value>
        /// Spatial and temporal extents.
        /// </value>
        [JsonProperty("extent")]
        public StacExtent Extent { get; internal set; }

        /// <summary>
        /// Gets list of keywords describing the Collection.
        /// </summary>
        /// <value>
        /// List of keywords describing the Collection.
        /// </value>
        [JsonProperty("keywords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Collection<string> Keywords { get; internal set; }

        [JsonProperty("assets")]
        public IDictionary<string, StacAsset> Assets { get; internal set; }

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

            this.StacExtensions = new SortedSet<string>(this.StacExtensions);
        }

#pragma warning disable 1591
        public bool ShouldSerializeSummaries()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.Summaries.Count > 0;
        }

#pragma warning disable 1591
        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.StacExtensions.Count > 0;
        }

#pragma warning disable 1591
        public bool ShouldSerializeAssets()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.Assets.Count > 0;
        }

        /// <summary>
        /// Generate a collection corresponding to the items' dictionary. Spatial and temporal extents
        /// are computed. Fields values are summarized in stats object and value sets.
        /// All Items are updated with the collection id
        /// </summary>
        /// <param name="id">Identifier of the collection</param>
        /// <param name="description">Description of the collection</param>
        /// <param name="items">Dictionary of Uri, StacItem. Uri points to the StacItem destination.</param>
        /// <param name="license">License of the collection</param>
        /// <param name="collectionUri">Uri of the collection. If provided, the items Uri and made relative to this one.</param>
        /// <param name="assets">Assets of the collection</param>
        public static StacCollection Create(
            string id,
            string description,
            IDictionary<Uri, StacItem> items,
            string license = null,
            Uri collectionUri = null,
            IDictionary<string, StacAsset> assets = null)
        {
            var collection = new StacCollection(
                                      id,
                                      description,
                                      StacExtent.Create(items.Values),
                                      assets,
                                      items.Select(item =>
                                      {
                                          Uri itemUri = item.Key;
                                          if (collectionUri != null)
                                          {
                                              itemUri = collectionUri.MakeRelativeUri(item.Key);
                                          }

                                          if (!itemUri.IsAbsoluteUri)
                                          {
                                              itemUri = new Uri("./" + itemUri.OriginalString, UriKind.Relative);
                                          }

                                          return StacLink.CreateObjectLink(item.Value, itemUri);
                                      }),
                                      license);

            var usedExtensions = items.SelectMany(item => item.Value.GetDeclaredExtensions());

            var summaryFunctions = usedExtensions.SelectMany(ext => ext.GetSummaryFunctions())
                .GroupBy(prop => prop.Key)
                .ToDictionary(key => key.Key, value => value.First().Value);

            summaryFunctions.Add("gsd", new SummaryFunction<double>(null, "gsd", StacPropertiesContainerExtension.CreateRangeSummaryObject));
            summaryFunctions.Add("platform", new SummaryFunction<string>(null, "platform", StacPropertiesContainerExtension.CreateSummaryValueSet));
            summaryFunctions.Add("constellation", new SummaryFunction<string>(null, "constellation", StacPropertiesContainerExtension.CreateSummaryValueSet));
            summaryFunctions.Add("instruments", new SummaryFunction<string>(null, "instruments", StacPropertiesContainerExtension.CreateSummaryValueSet));

            collection.Summaries =
                items.Values.SelectMany(item => item.Properties.Where(k => summaryFunctions.Keys.Contains(k.Key)))
                    .GroupBy(prop => prop.Key)
                    .ToDictionary(key => key.Key, value =>
                    {
                        if (summaryFunctions.ContainsKey(value.Key))
                        {
                            if (summaryFunctions[value.Key].Extension != null && !collection.StacExtensions.Contains(summaryFunctions[value.Key].Extension.Identifier))
                            {
                                collection.StacExtensions.Add(summaryFunctions[value.Key].Extension.Identifier);
                            }

                            return summaryFunctions[value.Key].Summarize(value.Select(i => i.Value));
                        }

                        return null;
                    });

            return collection;
        }

        public void Update(IDictionary<Uri, StacItem> items)
        {
            this.Extent.Update(items.Values);

            var usedExtensions = items.SelectMany(item => item.Value.GetDeclaredExtensions());

            var summaryFunctions = usedExtensions.SelectMany(ext => ext.GetSummaryFunctions())
                .GroupBy(prop => prop.Key)
                .ToDictionary(key => key.Key, value => value.First().Value);

            summaryFunctions.Add("gsd", new SummaryFunction<double>(null, "gsd", StacPropertiesContainerExtension.CreateRangeSummaryObject));
            summaryFunctions.Add("platform", new SummaryFunction<string>(null, "platform", StacPropertiesContainerExtension.CreateSummaryValueSet));
            summaryFunctions.Add("constellation", new SummaryFunction<string>(null, "constellation", StacPropertiesContainerExtension.CreateSummaryValueSet));
            summaryFunctions.Add("instruments", new SummaryFunction<string>(null, "instruments", StacPropertiesContainerExtension.CreateSummaryValueSet));

            this.Summaries =
                items.Values.SelectMany(item => item.Properties.Where(k => summaryFunctions.Keys.Contains(k.Key)))
                    .Concat(this.Summaries.SelectMany(s => s.Value.Enumerate().Select(v => new KeyValuePair<string, object>(s.Key, v))))
                    .GroupBy(prop => prop.Key)
                    .ToDictionary(key => key.Key, value =>
                    {
                        if (summaryFunctions.ContainsKey(value.Key))
                        {
                            if (summaryFunctions[value.Key].Extension != null && !this.StacExtensions.Contains(summaryFunctions[value.Key].Extension.Identifier))
                            {
                                this.StacExtensions.Add(summaryFunctions[value.Key].Extension.Identifier);
                            }

                            return summaryFunctions[value.Key].Summarize(value.Select(i => i.Value));
                        }

                        return null;
                    });

            return;
        }

        /// <summary>
        /// Clone this object.
        /// </summary>
        public object Clone()
        {
            return new StacCollection(this);
        }
    }
}
