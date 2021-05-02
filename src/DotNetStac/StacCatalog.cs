using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac.Converters;
using Newtonsoft.Json;
using Stac.Extensions;
using System.Net.Mime;
using Semver;
using System.Runtime.Serialization;

namespace Stac
{
    /// <summary>
    /// STAC Catalog Object implementing STAC Catalog spec (https://github.com/radiantearth/stac-spec/blob/master/catalog-spec/catalog-spec.md)
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacCatalog : IStacObject, IStacParent, IStacCatalog
    {
        /// <summary>
        /// Catalog Media-Type string
        /// </summary>
        public const string MEDIATYPE = "application/json; profile=stac-catalog";

        /// <summary>
        /// Catalog Media-Type Object
        /// </summary>
        /// <returns></returns>
        public readonly static ContentType CATALOG_MEDIATYPE = new ContentType(MEDIATYPE);

        /// <summary>
        /// Initialize an empty STAC Catalog
        /// </summary>
        /// <param name="id">required identifier of the catalog</param>
        /// <param name="description">required description of the catalog</param>
        /// <param name="links">optional links of the catalog</param>
        [JsonConstructor]
        public StacCatalog(string id, string description, IEnumerable<StacLink> links = null)
        {
            this.Id = id;
            this.StacVersion = Versions.StacVersionList.Current;
            this.Description = description;
            if (links == null)
                this.Links = new Collection<StacLink>();
            else
                this.Links = new Collection<StacLink>(links.ToList());
            this.Properties = new Dictionary<string, object>();
            this.Summaries = new Dictionary<string, Stac.Collection.IStacSummaryItem>();
            this.StacExtensions = new Collection<string>();
        }

        # region IStacObject

        /// <summary>
        /// Identifier for the Catalog.
        /// </summary>
        /// <value></value>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// The STAC version the Catalog implements
        /// </summary>
        /// <value></value>
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// A list of extension identifiers the Catalog implements
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
        public ContentType MediaType => CATALOG_MEDIATYPE;

        # endregion IStacObject

        /// <summary>
        /// STAC type (Catalog)
        /// </summary>
        [JsonProperty("type")]
        public string Type => "Catalog";

        /// <summary>
        /// Detailed multi-line description to fully explain the Catalog. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value></value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A short descriptive one-line title for the Catalog.
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
        /// Catalog Properties
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; internal set; }

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
