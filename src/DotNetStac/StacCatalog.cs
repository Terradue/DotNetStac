// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacCatalog.cs

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Semver;
using Stac.Converters;

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
        public const string MEDIATYPE = "application/json";

        /// <summary>
        /// Catalog Media-Type Object
        /// </summary>
        public static readonly ContentType CATALOG_MEDIATYPE = new ContentType(MEDIATYPE);

        /// <summary>
        /// Initializes a new instance of the <see cref="StacCatalog"/> class.
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
            {
                this.Links = new Collection<StacLink>();
            }
            else
            {
                this.Links = new Collection<StacLink>(links.ToList());
            }

            this.Summaries = new Dictionary<string, Collection.IStacSummaryItem>();
            this.StacExtensions = new SortedSet<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacCatalog"/> class.
        /// Initialize a new Stac Catalog from an existing one (clone)
        /// </summary>
        /// <param name="stacCatalog">existing Stac Catalog</param>
        public StacCatalog(StacCatalog stacCatalog)
        {
            this.Id = stacCatalog.Id;
            this.StacExtensions = new SortedSet<string>(stacCatalog.StacExtensions);
            this.StacVersion = stacCatalog.StacVersion;
            this.Links = new Collection<StacLink>(stacCatalog.Links.ToList());
            this.Summaries = new Dictionary<string, Collection.IStacSummaryItem>(stacCatalog.Summaries);
            this.Properties = new Dictionary<string, object>(stacCatalog.Properties);
        }

        /// <summary>
        /// Gets identifier for the Catalog.
        /// </summary>
        /// <value>
        /// Identifier for the Catalog.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets or sets the STAC version the Catalog implements
        /// </summary>
        /// <value>
        /// The STAC version the Catalog implements
        /// </value>
        [JsonProperty("stac_version")]
        [JsonConverter(typeof(SemVersionConverter))]
        public SemVersion StacVersion { get; set; }

        /// <summary>
        /// Gets a list of extension identifiers the Catalog implements
        /// </summary>
        /// <value>
        /// A list of extension identifiers the Catalog implements
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
        public ContentType MediaType => CATALOG_MEDIATYPE;

        /// <summary>
        /// Gets sTAC type (Catalog)
        /// </summary>
        /// <value>
        /// STAC type (Catalog)
        /// </value>
        [JsonProperty("type")]
        public string Type => "Catalog";

        /// <summary>
        /// Gets a map of property summaries, either a set of values or statistics such as a range.
        /// </summary>
        /// <value>
        /// A map of property summaries, either a set of values or statistics such as a range.
        /// </value>
        [JsonProperty("summaries")]
        [JsonConverter(typeof(StacSummariesConverter))]
        public Dictionary<string, Collection.IStacSummaryItem> Summaries { get; internal set; }

        /// <summary>
        /// Gets catalog Properties
        /// </summary>
        /// <value>
        /// Catalog Properties
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; internal set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IStacObject StacObjectContainer => this;

#pragma warning disable 1591
        public bool ShouldSerializeSummaries()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.Summaries.Count > 0;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach (StacLink link in this.Links)
            {
                link.Parent = this;
            }

            this.StacExtensions = new SortedSet<string>(this.StacExtensions);
        }

        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.StacExtensions.Count > 0;
        }
    }
}
