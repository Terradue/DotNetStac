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

        Uri href;
        string rel, title, type;
        private IStacObject hostObject;
        private readonly ulong contentLength;

        public StacLink()
        {
        }

        public StacLink(Uri uri)
        {
            Uri = uri;
        }

        public StacLink(Uri uri, IStacObject hostObject)
        {
            Uri = uri;
            this.hostObject = hostObject;
        }

        public StacLink(Uri uri, string relationshipType, string title, string mediaType, ulong contentLength = 0)
        {
            Uri = uri;
            RelationshipType = relationshipType;
            Title = title;
            MediaType = mediaType;
            this.contentLength = contentLength;
        }

        public StacLink(StacLink source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            href = source.href;
            rel = source.rel;
            title = source.title;
            type = source.type;
            hostObject = source.hostObject;
            contentLength = source.Length;
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

        [JsonIgnore]
        public Uri AbsoluteUri
        {
            get
            {
                if (Uri.IsAbsoluteUri)
                    return Uri;

                if (hostObject != null)
                    return new Uri(new Uri(hostObject.Uri.AbsoluteUri.Substring(0, hostObject.Uri.AbsoluteUri.LastIndexOf('/') + 1)), Uri);

                return null;
            }
        }

        public IStacObject Parent
        {
            get => hostObject;
            internal set
            {
                hostObject = value;
            }
        }

        public ulong Length => contentLength;

        public virtual StacLink Clone()
        {
            return new StacLink(this);
        }

    }
}
