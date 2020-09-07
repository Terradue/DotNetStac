using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac.Converters;
using Newtonsoft.Json;

namespace Stac
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StacAsset
    {

        #region Static members

        public static StacAsset CreateThumbnailAsset(Uri uri, string mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "thumbnail" }, title, mediaType);
        }

        public static StacAsset CreateOverviewAsset(Uri uri, string mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "overview" }, title, mediaType);
        }

        public static StacAsset CreateDataAsset(Uri uri, string mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "data" }, title, mediaType);
        }

        public static StacAsset CreateMetadataAsset(Uri uri, string mediaType, string title = null)
        {
            return new StacAsset(uri, new string[] { "metadata" }, title, mediaType);
        }

        #endregion

        Uri base_uri, href;
        string title, type, description;

        Collection<string> semanticRoles;
        private IStacObject hostObject;
        private ulong contentLength;

        public StacAsset()
        {
        }

        public StacAsset(Uri uri, IStacObject hostObject)
        {
            Uri = uri;
            this.hostObject = hostObject;
        }

        public StacAsset(Uri uri, IEnumerable<string> semanticRoles, string title, string mediaType, ulong contentLength = 0)
        {
            Uri = uri;
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
        }

        [JsonProperty("type")]
        public string MediaType
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

        public virtual StacAsset Clone()
        {
            return new StacAsset(this);
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

        [JsonIgnore]
        public ulong ContentLength { get => contentLength; set => contentLength = value; }
    }
}