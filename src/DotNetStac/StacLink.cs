using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacLink
    {
        #region Static members

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

        #endregion


        public StacLink()
        {
        }

        public StacLink(Uri uri)
        {
            Uri = uri;
        }

        public StacLink(Uri uri, string relationshipType, string title, string mediaType, ulong contentLength = 0)
        {
            Uri = uri;
            RelationshipType = relationshipType;
            Title = title;
            ContentType = mediaType == null ? null : new ContentType(mediaType);
            this.Length = contentLength;
        }

        public StacLink(StacLink source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            Uri = source.Uri;
            RelationshipType = source.RelationshipType;
            Title = source.Title;
            ContentType = source.ContentType;
            Parent = source.Parent;
            Length = source.Length;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public virtual ContentType ContentType { get; set; }

        [JsonProperty("rel")]
        public virtual string RelationshipType { get; set; }

        [JsonProperty("title")]
        public virtual string Title { get; set; }

        [JsonProperty("href")]
        public virtual Uri Uri { get; set; }

        [JsonIgnore]
        public IStacObject Parent { get; set; }

        [JsonIgnore]
        public ulong Length { get; set; }

        public virtual StacLink Clone()
        {
            return new StacLink(this);
        }

    }
}
