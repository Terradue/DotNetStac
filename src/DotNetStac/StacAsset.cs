using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Stac.Converters;

namespace Stac
{
    /// <summary>
    /// STAC Asset Object implementing <seealso href="https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md#asset-object">STAC Asset</seealso>
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacAsset : IStacPropertiesContainer
    {

        #region Static members

        /// <summary>
        /// Create a thumbnail asset
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">Uri to the thumbnail</param>
        /// <param name="mediaType">media type of the thumbnail</param>
        /// <param name="title">title of the thumbnail (if any)</param>
        /// <returns>A new Asset describing the thumbnail</returns>
        public static StacAsset CreateThumbnailAsset(IStacObject stacObject, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacObject, uri, new string[] { "thumbnail" }, title, mediaType);
        }

        /// <summary>
        /// Create an overview asset
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">Uri to the overview</param>
        /// <param name="mediaType">media type of the overview</param>
        /// <param name="title">title of the overview (if any)</param>
        /// <returns>A new Asset describing the overview</returns>
        public static StacAsset CreateOverviewAsset(IStacObject stacObject, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacObject, uri, new string[] { "overview" }, title, mediaType);
        }

        /// <summary>
        /// Create a data asset
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">Uri to the data</param>
        /// <param name="mediaType">media type of the data</param>
        /// <param name="title">title of the data (if any)</param>
        /// <returns>A new Asset describing the data</returns>
        public static StacAsset CreateDataAsset(IStacObject stacObject, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacObject, uri, new string[] { "data" }, title, mediaType);
        }

        /// <summary>
        /// Create a metadata asset
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">Uri to the metadata</param>
        /// <param name="mediaType">media type of the metadata</param>
        /// <param name="title">title of the metadata (if any)</param>
        /// <returns>A new Asset describing the metadata</returns>
        public static StacAsset CreateMetadataAsset(IStacObject stacObject, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacObject, uri, new string[] { "metadata" }, title, mediaType);
        }

        #endregion

        Uri base_uri, href;
        string title, description;

        ContentType type;

        Collection<string> roles;
        private Dictionary<string, object> properties;
        private IStacObject parentStacObject;

        [JsonConstructor]
        internal StacAsset()
        {
            properties = new Dictionary<string, object>();
            Roles = new SortedSet<string>();
        }

        /// <summary>
        /// Initialize a new asset with a Uri
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">uri to the asset</param>
        public StacAsset(IStacObject stacObject, Uri uri) : this()
        {
            if (!(stacObject is StacItem || stacObject is StacCollection))
                throw new InvalidOperationException("An asset cannot be defined in " + stacObject.GetType().Name);
            parentStacObject = stacObject;
            Uri = uri;

        }

        /// <summary>
        /// Initialize a new asset
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">uri to the asset</param>
        /// <param name="roles">roles of the asset</param>
        /// <param name="title">title of the asset</param>
        /// <param name="mediaType">media-type of the asset</param>
        public StacAsset(IStacObject stacObject, Uri uri, IEnumerable<string> roles, string title, ContentType mediaType) : this(stacObject, uri)
        {
            Roles = roles == null ? new Collection<string>() : new Collection<string>(roles.ToList());
            Title = title;
            MediaType = mediaType;
        }

        /// <summary>
        /// Initialize a new asset from an existing one
        /// </summary>
        /// <param name="source">asset source to be copied</param>
        /// <param name="stacObject">new parent stac object</param>
        public StacAsset(StacAsset source, IStacObject stacObject)
        {
            if (!(stacObject is StacItem || stacObject is StacCollection))
                throw new InvalidOperationException("An asset cannot be defined in " + stacObject.GetType().Name);
            if (source == null)
                throw new ArgumentNullException("source");
            base_uri = source.base_uri;
            href = source.href;
            if (source.roles != null)
                roles = new Collection<string>(source.roles);
            title = source.title;
            type = source.type;
            description = source.description;
            if (source.properties != null)
                properties = new Dictionary<string, object>(source.properties);
            parentStacObject = stacObject;
        }

        /// <summary>
        /// Media type of the asset
        /// </summary>
        /// <value></value>
        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public ContentType MediaType
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// The semantic roles of the asset
        /// </summary>
        /// <value></value>
        [JsonProperty("roles")]
        public ICollection<string> Roles
        {
            get;
            private set;
        }

        /// <summary>
        /// The displayed title for clients and users.
        /// </summary>
        /// <value></value>
        [JsonProperty("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        ///  URI to the asset object. Relative and absolute URI are both allowed.
        /// </summary>
        /// <value></value>
        [JsonProperty("href")]
        public Uri Uri
        {
            get { return href; }
            set { href = value; }
        }

        /// <summary>
        /// A description of the Asset providing additional details, such as how it was processed or created.
        /// </summary>
        /// <value></value>
        [JsonProperty("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Extended properties
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties
        {
            get
            {
                return properties;
            }

            set
            {
                properties = new Dictionary<string, object>(value);
            }
        }

        /// <summary>
        /// Object container
        /// </summary>
        [JsonIgnore]
        public IStacObject StacObjectContainer => ParentStacObject;

        /// <summary>
        /// parent stac object
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public IStacObject ParentStacObject { get => parentStacObject; internal set => parentStacObject = value; }

#pragma warning disable 1591
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return Roles.Count > 0;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Roles = new SortedSet<string>(Roles);
        }
    }
}
