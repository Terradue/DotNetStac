using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacLink
    {
        #region Static members

        public static StacLink CreateSelfLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "self", null, mediaType);
        }

        public static StacLink CreateRootLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "root", null, mediaType);
        }

        public static StacLink CreateParentLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "parent", null, mediaType);
        }

        public static StacLink CreateCollectionLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "collection", null, mediaType);
        }

        public static StacLink CreateDerivedFromLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "derived_from", null, mediaType);
        }

        public static StacLink CreateAlternateLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "alternate", null, mediaType);
        }

        #endregion

        Uri base_uri, href;
        string rel, title, type;

        public StacLink()
        {
        }

        public StacLink(Uri uri)
        {
            Uri = uri;
        }

        public StacLink(Uri uri, string relationshipType, string title, string mediaType)
        {
            Uri = uri;
            RelationshipType = relationshipType;
            Title = title;
            MediaType = mediaType;
        }

        public StacLink(StacLink source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            base_uri = source.base_uri;
            href = source.href;
            rel = source.rel;
            title = source.title;
            type = source.type;
        }

        [JsonIgnore]
        public Uri BaseUri
        {
            get { return base_uri; }
            set
            {
                if (value != null && !value.IsAbsoluteUri)
                    throw new ArgumentException("Base URI must not be relative");
                base_uri = value;
            }
        }

        [JsonProperty("type")]
        public string MediaType
        {
            get { return type; }
            set { type = value; }
        }

        [JsonProperty("rel")]
        public string RelationshipType
        {
            get { return rel; }
            set { rel = value; }
        }

        [JsonProperty("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [JsonProperty("href")]
        public Uri Uri
        {
            get { return href; }
            set { href = value; }
        }

        public virtual StacLink Clone()
        {
            return new StacLink(this);
        }

    }
}
