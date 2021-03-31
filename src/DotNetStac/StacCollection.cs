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

namespace Stac
{
    /// <summary>
    /// STAC Collection Object implementing STAC Collection spec (https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md)
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class StacCollection : IStacObject, IStacParent
    {
        public const string MEDIATYPE = "application/json; profile=stac-collection";
        public readonly static ContentType COLLECTION_MEDIATYPE = new ContentType(MEDIATYPE);


        [JsonConstructor]
        public StacCollection(string id,
                              string description,
                              StacExtent extent,
                              IDictionary<string, StacAsset> assets = null,
                              IEnumerable<StacLink> links = null,
                              string license = "proprietary")
        {
            this.Id = id;
            this.StacVersion = Versions.StacVersionList.Current;
            this.Description = description;
            if (links == null)
                this.Links = new Collection<StacLink>();
            else
                this.Links = new Collection<StacLink>(links.ToList());
            this.Properties = new Dictionary<string, object>();
            if (assets == null)
                this.Assets = new Dictionary<string, StacAsset>();
            else
                this.Assets = new Dictionary<string, StacAsset>(assets);
            this.Summaries = new Dictionary<string, Stac.Collection.IStacSummaryItem>();
            this.StacExtensions = new Collection<string>();
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
        public Collection<string> StacExtensions { get; private set; }

        /// <summary>
        /// A list of references to other documents.
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links")]
        public Collection<StacLink> Links
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
        public string Type => "Collection";

        /// <summary>
        /// Detailed multi-line description to fully explain the Collection. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value></value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A short descriptive one-line title for the Collection.
        /// </summary>
        /// <value></value>
        [JsonProperty("title")]
        public string Title { get; set; }

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
        /// Collection's license(s), either a SPDX License identifier, various if multiple licenses apply or proprietary for all other cases.
        /// </summary>
        /// <value></value>
        [JsonProperty("license")]
        public string License { get; set; }

        /// <summary>
        /// A list of providers, which may include all organizations capturing or processing the data or the hosting provider. 
        /// Providers should be listed in chronological order with the most recent provider being the last element of the list.
        /// </summary>
        /// <value></value>
        [JsonProperty("providers")]
        public Collection<StacProvider> Providers { get; internal set; }

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
        }

        public bool ShouldSerializeSummaries()
        {
            // don't serialize the Manager property if an employee is their own manager
            return Summaries.Count > 0;
        }

        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return StacExtensions.Count > 0;
        }
    }
}
