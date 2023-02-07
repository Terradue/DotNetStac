// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: AlternateAssetObject.cs

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
        private string href;
        private readonly IStacObject _parent;
        private string title;

        private string description;
        private IDictionary<string, object> properties;

        /// <summary>
        /// Initialize a new Band Object
        /// </summary>
        /// <param name="href">URI to the asset object</param>
        /// <param name="parent">Parent object</param>
        /// <param name="title">The displayed title for clients and users.</param>
        /// <param name="description">A description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.</param>
        public AlternateAssetObject(string href, IStacObject parent = null, string title = null, string description = null)
        {
            this.href = href;
            this._parent = parent;
            this.title = title;
            this.description = description;
            this.properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets rEQUIRED. URI to the asset object. Relative and absolute URI are both allowed.
        /// </summary>
        /// <value>
        /// <placeholder>REQUIRED. URI to the asset object. Relative and absolute URI are both allowed.</placeholder>
        /// </value>
        [JsonProperty("href")]
        [JsonRequired]
        public string Href { get => this.href; set => this.href = value; }

        /// <summary>
        /// Gets or sets the displayed title for clients and users.
        /// </summary>
        /// <value>
        /// <placeholder>The displayed title for clients and users.</placeholder>
        /// </value>
        [JsonProperty("title")]
        public string Title { get => this.title; set => this.title = value; }

        /// <summary>
        /// Gets or sets a description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// <placeholder>A description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.</placeholder>
        /// </value>
        [JsonProperty("description")]
        public string Description { get => this.description; set => this.description = value; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>

        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this.properties; set => this.properties = value; }

        /// <summary>
        /// Gets parent Stac Object
        /// </summary>
        /// <value>
        /// <placeholder>Parent Stac Object</placeholder>
        /// </value>

        /// <value>
        /// <placeholder>Parent Stac Object</placeholder>
        /// </value>
        [JsonIgnore]
        public IStacObject StacObjectContainer => this._parent;

        /// <summary>
        /// Gets uri
        /// </summary>
        /// <value>
        /// <placeholder>Uri</placeholder>
        /// </value>

        /// <value>
        /// <placeholder>Uri</placeholder>
        /// </value>
        [JsonIgnore]
        public Uri Uri => new Uri(this.href);
    }
}
