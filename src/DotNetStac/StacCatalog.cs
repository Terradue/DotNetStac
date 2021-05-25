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
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    public partial class StacCatalog : IStacObject, IStacParent, IStacCatalog
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
            this.Properties = new Dictionary<string, object>();
            this.Id = id;
            this.StacVersion = Versions.StacVersionList.Current;
            this.Description = description;
            if (links == null)
                this.Links = new Collection<StacLink>();
            else
                this.Links = new Collection<StacLink>(links.ToList());
            this.Summaries = new Dictionary<string, Stac.Collection.IStacSummaryItem>();
            this.StacExtensions = new SortedSet<string>();
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
            StacExtensions = new SortedSet<string>(StacExtensions);
        }

#pragma warning disable 1591
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
