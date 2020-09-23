using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac.Converters;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacAsset
    {

        #region Static members

        public static StacAsset CreateThumbnailAsset(Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "thumbnail" }, title, mediaType);
        }

        public static StacAsset CreateOverviewAsset(Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "overview" }, title, mediaType);
        }

        public static StacAsset CreateDataAsset(Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "data" }, title, mediaType);
        }

        public static StacAsset CreateMetadataAsset(Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "metadata" }, title, mediaType);
        }

        #endregion

        Uri base_uri, href;
        string title, description;

        ContentType type;

        Collection<string> semanticRoles;
        private ulong contentLength;
        private IDictionary<string, object> properties;

        public StacAsset()
        {
            properties = new Dictionary<string, object>();
        }

        public StacAsset(Uri uri) : this()
        {
            Uri = uri;
        }

        public StacAsset(Uri uri, IEnumerable<string> semanticRoles, string title, ContentType mediaType, ulong contentLength = 0) : this (uri)
        {
            this.semanticRoles = semanticRoles == null ? new Collection<string>() : new Collection<string>(semanticRoles.ToList());
            Title = title;
            MediaType = mediaType;
            ContentLength = contentLength;
        }

        public StacAsset(StacAsset source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            base_uri = source.base_uri;
            href = source.href;
            semanticRoles = source.semanticRoles;
            title = source.title;
            type = source.type;
            description = source.description;
            contentLength = source.contentLength;
            properties = source.properties;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(ContentTypeConverter))]
        public ContentType MediaType
        {
            get { return type; }
            set { type = value; }
        }

        [JsonProperty("roles")]
        public List<string> Roles
        {
            get
            {
                if (semanticRoles == null || semanticRoles.Count == 0)
                    return null;
                return semanticRoles.ToList();
            }
            set
            {
                if (value == null)
                    semanticRoles = null;
                else
                    semanticRoles = new Collection<string>(value);
            }
        }

        [JsonIgnore]
        public Collection<string> SemanticRoles
        {
            get
            {
                if (semanticRoles == null)
                    semanticRoles = new Collection<string>();
                return semanticRoles;
            }
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

        [JsonProperty("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [JsonExtensionData]
        public IDictionary<string, object> Properties
        {
            get
            {
                return properties;
            }

            set
            {
                properties = value;
            }
        }


        public virtual StacAsset Clone()
        {
            return new StacAsset(this);
        }

        [JsonIgnore]
        public ulong ContentLength { get => contentLength; set => contentLength = value; }
    }
}