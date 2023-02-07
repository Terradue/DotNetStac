// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacAsset.cs

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Stac
{
    /// <summary>
    /// STAC Asset Object implementing <seealso href="https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md#asset-object">STAC Asset</seealso>.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacAsset : IStacPropertiesContainer
    {
        private readonly Uri _base_uri;
        private Uri _href;
        private string _title, _description;
        private ContentType _type;
        private Dictionary<string, object> _properties;
        private IStacObject _parentStacObject;

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

        [JsonConstructor]
        internal StacAsset()
        {
            this._properties = new Dictionary<string, object>();
            this.Roles = new SortedSet<string>();
        }

        /// <summary>
        /// Initialize a new asset with a Uri
        /// </summary>
        /// <param name="stacObject">parent stac object</param>
        /// <param name="uri">uri to the asset</param>
        public StacAsset(IStacObject stacObject, Uri uri) : this()
        {
            if (!(stacObject == null || stacObject is StacItem || stacObject is StacCollection))
            {
                throw new InvalidOperationException("An asset cannot be defined in " + stacObject.GetType().Name);
            }

            this._parentStacObject = stacObject;
            this.Uri = uri;
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
            this.Roles = roles == null ? new SortedSet<string>() : new SortedSet<string>(roles.ToList());
            this.Title = title;
            this.MediaType = mediaType;
        }

        /// <summary>
        /// Initialize a new asset from an existing one
        /// </summary>
        /// <param name="source">asset source to be copied</param>
        /// <param name="stacObject">new parent stac object</param>
        public StacAsset(StacAsset source, IStacObject stacObject)
        {
            if (!(stacObject == null || stacObject is StacItem || stacObject is StacCollection))
            {
                throw new InvalidOperationException("An asset cannot be defined in " + stacObject.GetType().Name);
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this._base_uri = source._base_uri;
            this._href = source._href;
            if (source.Roles != null)
            {
                this.Roles = new SortedSet<string>(source.Roles);
            }
            else
            {
                this.Roles = new SortedSet<string>();
            }

            this._title = source._title;
            this._type = source._type;
            this._description = source._description;
            if (source._properties != null)
            {
                this._properties = new Dictionary<string, object>(source._properties);
            }

            this._parentStacObject = stacObject;
        }

        /// <summary>
        /// Gets or sets media type of the asset
        /// </summary>
        /// <value></value>
        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public ContentType MediaType
        {
            get { return this._type; }
            set { this._type = value; }
        }

        /// <summary>
        /// Gets the semantic roles of the asset
        /// </summary>
        /// <value></value>
        [JsonProperty("roles")]
        public ICollection<string> Roles
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the displayed title for clients and users.
        /// </summary>
        /// <value></value>
        [JsonProperty("title")]
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        /// <summary>
        ///  Gets or sets uRI to the asset object. Relative and absolute URI are both allowed.
        /// </summary>
        /// <value></value>
        [JsonProperty("href")]
        public Uri Uri
        {
            get { return this._href; }
            set { this._href = value; }
        }

        /// <summary>
        /// Gets or sets a description of the Asset providing additional details, such as how it was processed or created.
        /// </summary>
        /// <value></value>
        [JsonProperty("description")]
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        /// <summary>
        /// Gets or sets extended properties
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties
        {
            get
            {
                return this._properties;
            }

            set
            {
                this._properties = new Dictionary<string, object>(value);
            }
        }

        /// <summary>
        /// Gets object container
        /// </summary>
        /// <value>
        /// <placeholder>Object container</placeholder>
        /// </value>
        [JsonIgnore]
        public IStacObject StacObjectContainer => this.ParentStacObject;

        /// <summary>
        /// Gets parent stac object
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public IStacObject ParentStacObject { get => this._parentStacObject; internal set => this._parentStacObject = value; }

#pragma warning disable 1591
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeStacExtensions()
        {
            // don't serialize the Manager property if an employee is their own manager
            return this.Roles.Count > 0;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            this.Roles = new SortedSet<string>(this.Roles);
        }
    }
}
