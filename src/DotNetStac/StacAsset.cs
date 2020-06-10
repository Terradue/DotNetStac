using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DotNetStac.Converters;
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

        public StacAsset()
        {
        }

        public StacAsset(Uri uri)
        {
            Uri = uri;
        }

        public StacAsset(Uri uri, IEnumerable<string> semanticRoles, string title, string mediaType)
        {
            Uri = uri;
            this.semanticRoles = semanticRoles == null ? new Collection<string>() : new Collection<string>(semanticRoles.ToList());
            Title = title;
            MediaType = mediaType;
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
    }
}