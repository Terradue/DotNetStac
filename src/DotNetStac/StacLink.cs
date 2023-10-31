// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacLink.cs

using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Stac
{
    /// <summary>
    /// A STAC link
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    [DataContract]
    public class StacLink : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StacLink"/> class.
        /// </summary>
        public StacLink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacLink"/> class.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        public StacLink(Uri uri)
        {
            this.Uri = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacLink"/> class.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="relationshipType">Relationship type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="contentLength">Content length of the link</param>
        public StacLink(Uri uri, string relationshipType, string title, string mediaType, ulong contentLength = 0)
        {
            this.Uri = uri;
            this.RelationshipType = relationshipType;
            this.Title = title;
            this.ContentType = mediaType == null ? null : new ContentType(mediaType);
            this.Length = contentLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacLink"/> class.
        /// </summary>
        /// <param name="source">Source StacLink to clone</param>
        public StacLink(StacLink source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.Uri = source.Uri;
            this.RelationshipType = source.RelationshipType;
            this.Title = source.Title;
            this.ContentType = source.ContentType;
            this.Parent = source.Parent;
            this.Length = source.Length;
            this.AdditionalProperties = source.AdditionalProperties;
        }

        /// <summary>
        /// Gets or sets the Content Type of the link using a string.
        /// </summary>
        [JsonProperty("type")]
        [DataMember(Name = "type")]
        public string Type
        {
            get => this.ContentType?.ToString();
            set => this.ContentType = value == null ? null : new ContentType(value);
        }

        /// <summary>
        /// Gets or sets the Content-Type of the link.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the relationship type of the link.
        /// </summary>
        [JsonProperty("rel", Required = Required.Always)]
        [DataMember(Name = "rel", IsRequired = true)]
        public virtual string RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the title of the link.
        /// </summary>
        [JsonProperty("title")]
        [DataMember(Name = "title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the URI of the link.
        /// </summary>
        [JsonProperty("href")]
        [DataMember(Name = "href", IsRequired = true)]
        public virtual Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the parent of the link.
        /// This is used to resolve relative links.
        /// </summary>
        [JsonIgnore]
        public IStacObject Parent { get; set; }

        /// <summary>
        /// Gets or sets the length of the link.
        /// </summary>
        [JsonIgnore]
        public ulong Length { get; set; }

        /// <summary>
        /// Gets or sets the additional properties of the link.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }

        /// <summary>
        /// Create a self link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateSelfLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "self", title, mediaType);
        }

        /// <summary>
        /// Create a root link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateRootLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "root", title, mediaType);
        }

        /// <summary>
        /// Create a parent link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateParentLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "parent", title, mediaType);
        }

        /// <summary>
        /// Create a collection link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateCollectionLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "collection", title, null);
        }

        /// <summary>
        /// Create a derived_from link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateDerivedFromLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "derived_from", title, mediaType);
        }

        /// <summary>
        /// Create an alternate link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateAlternateLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "alternate", title, mediaType);
        }

        /// <summary>
        /// Create a child link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateChildLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "child", title, mediaType);
        }

        /// <summary>
        /// Create an item link.
        /// </summary>
        /// <param name="uri">Uri of the link</param>
        /// <param name="mediaType">Media type of the link</param>
        /// <param name="title">Title of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateItemLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "item", title, mediaType);
        }

        /// <summary>
        /// Create a link from a StacObject.
        /// </summary>
        /// <param name="stacObject">StacObject to create a link from</param>
        /// <param name="uri">Uri of the link</param>
        /// <returns>StacLink</returns>
        public static StacLink CreateObjectLink(IStacObject stacObject, Uri uri)
        {
            return new StacObjectLink(stacObject, uri);
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new StacLink(this);
        }
    }
}
