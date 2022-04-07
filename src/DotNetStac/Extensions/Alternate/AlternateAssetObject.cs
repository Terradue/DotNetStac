using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac.Extensions.Alternate
{
    /// <summary>
    /// Represents the <seealso href="https://github.com/stac-extensions/alternate-assets#alternate-asset-object">Alternate Asset Object</seealso>
    /// of the Alternate extension
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AlternateAssetObject : IStacPropertiesContainer
    {
        private Uri uri;
        private readonly IStacObject _parent;
        private string title;

        private string description;

        IDictionary<string, object> properties;

        /// <summary>
        /// Initialize a new Band Object
        /// </summary>
        /// <param name="uri">URI to the asset object</param>
        /// <param name="parent">Parent object</param>
        /// <param name="title">The displayed title for clients and users.</param>
        /// <param name="description">A description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.</param>
        public AlternateAssetObject(Uri uri, IStacObject parent = null, string title = null, string description = null)
        {
            this.uri = uri;
            _parent = parent;
            this.title = title;
            this.description = description;
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// REQUIRED. URI to the asset object. Relative and absolute URI are both allowed.
        /// </summary>
        [JsonProperty("href")]
        [JsonRequired]
        public string Uri { get => uri.ToString(); set => uri = new Uri(value); }

        /// <summary>
        /// The displayed title for clients and users.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get => title; set => title = value; }

        /// <summary>
        /// A description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// Additional fields
        /// </summary>
        /// <value></value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => properties; set => properties = value; }

        /// <summary>
        /// Parent Stac Object
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public IStacObject StacObjectContainer => _parent;
    }
}
