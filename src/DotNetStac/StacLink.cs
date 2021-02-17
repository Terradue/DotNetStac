using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac.Converters;

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

        public static StacLink CreateChildLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "child", null, mediaType);
        }

        public static StacLink CreateItemLink(Uri uri, string mediaType = null)
        {
            return new StacLink(uri, "item", null, mediaType);
        }

        public static StacLink CreateItemLink(IStacObject stacObject, IStacObject hostObject = null)
        {
            return new StacObjectLink(stacObject, hostObject);
        }

        #endregion

        Uri href;
        protected string rel, title;
        ContentType type;
        protected IStacObject hostObject;
        protected readonly ulong contentLength;

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
            MediaType = mediaType == null ? null : new ContentType(mediaType);
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
        [JsonConverter(typeof(ContentTypeConverter))]
        public virtual ContentType MediaType
        {
            get { return type; }
            set { type = value; }
        }

        [JsonProperty("rel")]
        public virtual string RelationshipType
        {
            get { return rel; }
            set { rel = value; }
        }

        [JsonProperty("title")]
        public virtual string Title
        {
            get { return title; }
            set { title = value; }
        }

        [JsonProperty("href")]
        public virtual Uri Uri
        {
            get { return href; }
            set { href = value; }
        }

        [JsonIgnore]
        private Uri AbsoluteUri
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

        [JsonIgnore]
        public IStacObject Parent
        {
            get => hostObject;
            internal set
            {
                hostObject = value;
            }
        }

        [JsonIgnore]
        public ulong Length => contentLength;

        public virtual StacLink Clone()
        {
            return new StacLink(this);
        }

        public virtual async Task<IStacObject> LoadAsync()
        {
            if (AbsoluteUri == null)
                throw new FileNotFoundException(string.Format("Cannot load STAC object from link ({0}) : No absolute entry point", Uri));
            return await StacFactory.LoadUriAsync(AbsoluteUri);
        }

    }
}
