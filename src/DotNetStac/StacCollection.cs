using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stac.Converters;
using Newtonsoft.Json;
using System.Net.Mime;
using Stac.Extensions;
using Semver;
using System.Linq;
using System.Runtime.Serialization;
using Stac.Collection;
using System;

namespace Stac
{
    /// <summary>
    /// STAC Collection Object implementing STAC Collection spec (https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md)
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    public partial class StacCollection : IStacObject, IStacParent, IStacCatalog
    {
        public const string MEDIATYPE = "application/json";
        public readonly static ContentType COLLECTION_MEDIATYPE = new ContentType(MEDIATYPE);


        [JsonConstructor]
        public StacCollection(string id,
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
                this.Links = new Collection<StacLink>();
            else
                this.Links = new Collection<StacLink>(links.ToList());
            if (assets == null)
                this.Assets = new Dictionary<string, StacAsset>();
            else
                this.Assets = new Dictionary<string, StacAsset>(assets);
            this.Summaries = new Dictionary<string, Stac.Collection.IStacSummaryItem>();
            this.StacExtensions = new SortedSet<string>();
            this.Providers = new Collection<StacProvider>();
            this.License = license;
            this.Keywords = new Collection<string>();
            this.Extent = extent;
        }

        # region IStacObject

        /// <summary>
        /// Identifier for the Collection.
        /// </summary>
        /// <value></value>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// The STAC version the Collection implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// A list of extension identifiers the Collection implements
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
            get; internal set;
        }

        [JsonIgnore]
        public ContentType MediaType => COLLECTION_MEDIATYPE;

        # endregion IStacObject

        /// <summary>
        /// STAC type (Collection)
        /// </summary>
        [JsonProperty("type")]
        public virtual string Type => "Collection";

        /// <summary>
        /// A map of property summaries, either a set of values or statistics such as a range.
        /// </summary>
        /// <value></value>
        [JsonProperty("summaries")]
        [JsonConverter(typeof(StacSummariesConverter))]
        public Dictionary<string, Stac.Collection.IStacSummaryItem> Summaries { get; internal set; }

        /// <summary>
        /// Collection extended data
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; internal set; }

        /// <summary>
        /// Spatial and temporal extents.
        /// </summary>
        /// <value></value>
        [JsonProperty("extent")]
        public StacExtent Extent { get; internal set; }

        /// <summary>
        /// List of keywords describing the Collection.
        /// </summary>
        /// <value></value>
        [JsonProperty("keywords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Collection<string> Keywords { get; internal set; }

        [JsonProperty("assets")]
        public IDictionary<string, StacAsset> Assets { get; internal set; }

        [JsonIgnore]
        public IStacObject StacObjectContainer => this;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in Links)
            {
                link.Parent = this;
            }
            StacExtensions = new SortedSet<string>(StacExtensions);
        }

#pragma warning disable 1591
        public bool ShouldSerializeSummaries()
        {
            // don't serialize the Manager property if an employee is their own manager
            return Summaries.Count > 0;
        }

#pragma warning disable 1591
        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return StacExtensions.Count > 0;
        }

#pragma warning disable 1591
        public bool ShouldSerializeAssets()
        {
            // don't serialize the Manager property if an employee is their own manager
            return Assets.Count > 0;
        }

        #region Static Methods

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
        /// <returns></returns>
        public static StacCollection Create(string id,
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
                                              itemUri = collectionUri.MakeRelativeUri(item.Key);
                                          if (!itemUri.IsAbsoluteUri) { itemUri = new Uri("./" + itemUri.OriginalString, UriKind.Relative); }
                                          return StacLink.CreateObjectLink(item.Value, itemUri);
                                      }),
                                      license);

            var usedExtensions = items.SelectMany(item => item.Value.GetDeclaredExtensions());

            var summaryFunctions = usedExtensions.SelectMany(ext => ext.GetSummaryFunctions())
                .GroupBy(prop => prop.Key)
                .ToDictionary(key => key.Key, value => value.First().Value);

            summaryFunctions.Add("gsd", new SummaryFunction(null, "gsd", StacPropertiesContainerExtension.CreateRangeSummaryObject));
            summaryFunctions.Add("platform", new SummaryFunction(null, "platform", StacPropertiesContainerExtension.CreateSummaryValueSet));
            summaryFunctions.Add("constellation", new SummaryFunction(null, "constellation", StacPropertiesContainerExtension.CreateSummaryValueSet));
            summaryFunctions.Add("instruments", new SummaryFunction(null, "instruments", StacPropertiesContainerExtension.CreateSummaryValueSetFromArrays));

            collection.Summaries =
                items.Values.SelectMany(item => item.Properties.Where(k => summaryFunctions.Keys.Contains(k.Key)))
                    .GroupBy(prop => prop.Key)
                    .ToDictionary(key => key.Key, value =>
                    {
                        if (summaryFunctions[value.Key].Extension != null && !collection.StacExtensions.Contains(summaryFunctions[value.Key].Extension.Identifier))
                            collection.StacExtensions.Add(summaryFunctions[value.Key].Extension.Identifier);
                        return summaryFunctions[value.Key].Summarize(value.Select(i => i.Value));
                    });

            foreach (var item in items)
            {
                item.Value.SetCollection(id, collectionUri);
            }

            return collection;
        }

        #endregion
    }
}
