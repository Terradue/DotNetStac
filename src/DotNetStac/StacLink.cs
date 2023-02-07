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
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    [DataContract]
    public class StacLink
    {
        public StacLink()
        {
        }

        public StacLink(Uri uri)
        {
            this.Uri = uri;
        }

        public StacLink(Uri uri, string relationshipType, string title, string mediaType, ulong contentLength = 0)
        {
            this.Uri = uri;
            this.RelationshipType = relationshipType;
            this.Title = title;
            this.ContentType = mediaType == null ? null : new ContentType(mediaType);
            this.Length = contentLength;
        }

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
        }

        public static StacLink CreateSelfLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "self", title, mediaType);
        }

        public static StacLink CreateRootLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "root", title, mediaType);
        }

        public static StacLink CreateParentLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "parent", title, mediaType);
        }

        public static StacLink CreateCollectionLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "collection", title, null);
        }

        public static StacLink CreateDerivedFromLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "derived_from", title, mediaType);
        }

        public static StacLink CreateAlternateLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "alternate", title, mediaType);
        }

        public static StacLink CreateChildLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "child", title, mediaType);
        }

        public static StacLink CreateItemLink(Uri uri, string mediaType = null, string title = null)
        {
            return new StacLink(uri, "item", title, mediaType);
        }

        public static StacLink CreateObjectLink(IStacObject stacObject, Uri uri)
        {
            return new StacObjectLink(stacObject, uri);
        }

        [JsonProperty("type")]
        [DataMember(Name = "type")]
        public string Type
        {
            get => this.ContentType?.ToString();
            set => this.ContentType = value == null ? null : new ContentType(value);
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ContentType ContentType { get; set; }

        [JsonProperty("rel", Required = Required.Always)]
        [DataMember(Name = "rel", IsRequired = true)]
        public virtual string RelationshipType { get; set; }

        [JsonProperty("title")]
        [DataMember(Name = "title")]
        public virtual string Title { get; set; }

        [JsonProperty("href")]
        [DataMember(Name = "href", IsRequired = true)]
        public virtual Uri Uri { get; set; }

        [JsonIgnore]
        public IStacObject Parent { get; set; }

        [JsonIgnore]
        public ulong Length { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }

        public virtual StacLink Clone()
        {
            return new StacLink(this);
        }
    }
}
