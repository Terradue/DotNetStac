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
    public class StacAsset : IStacPropertiesContainer
    {

        #region Static members

        public static StacAsset CreateThumbnailAsset(StacItem stacItem, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacItem, uri, new string[] { "thumbnail" }, title, mediaType);
        }

        public static StacAsset CreateOverviewAsset(StacItem stacItem, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacItem, uri, new string[] { "overview" }, title, mediaType);
        }

        public static StacAsset CreateDataAsset(StacItem stacItem, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacItem, uri, new string[] { "data" }, title, mediaType);
        }

        public static StacAsset CreateMetadataAsset(StacItem stacItem, Uri uri, ContentType mediaType, string title = null)
        {
            return new StacAsset(stacItem, uri, new string[] { "metadata" }, title, mediaType);
        }

        #endregion

        Uri base_uri, href;
        string title, description;

        ContentType type;

        Collection<string> semanticRoles;
        private Dictionary<string, object> properties;
        private StacItem parentStacItem;

        [JsonConstructor]
        public StacAsset(StacItem stacItem)
        {
            properties = new Dictionary<string, object>();
            parentStacItem = stacItem;
        }

        public StacAsset(StacItem stacItem, Uri uri) : this(stacItem)
        {
            Uri = uri;
        }

        public StacAsset(StacItem stacItem, Uri uri, IEnumerable<string> semanticRoles, string title, ContentType mediaType, ulong contentLength = 0) : this(stacItem, uri)
        {
            this.semanticRoles = semanticRoles == null ? new Collection<string>() : new Collection<string>(semanticRoles.ToList());
            Title = title;
            MediaType = mediaType;
            if (contentLength > 0)
                ContentLength = contentLength;
        }

        public StacAsset(StacAsset source, StacItem stacItem)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            base_uri = source.base_uri;
            href = source.href;
            if (source.semanticRoles != null)
                semanticRoles = new Collection<string>(source.semanticRoles);
            title = source.title;
            type = source.type;
            description = source.description;
            if (source.properties != null)
                properties = new Dictionary<string, object>(source.properties);
            parentStacItem = stacItem;
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
                properties = new Dictionary<string, object>(value);
            }
        }

        [JsonIgnore]
        public ulong ContentLength { get => this.GetProperty<ulong>("size"); set => this.SetProperty("size", value); }

        [JsonIgnore]
        public IStacObject StacObjectContainer => ParentStacItem;

        [JsonIgnore]
        public StacItem ParentStacItem { get => parentStacItem; internal set => parentStacItem = value; }
    }
}