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
        private readonly IStacObject _parent;
        private string _href;
        private string _title;

        private string _description;
        private IDictionary<string, object> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateAssetObject"/> class.
        /// </summary>
        /// <param name="href">URI to the asset object</param>
        /// <param name="parent">Parent object</param>
        /// <param name="title">The displayed title for clients and users.</param>
        /// <param name="description">A description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.</param>
        public AlternateAssetObject(string href, IStacObject parent = null, string title = null, string description = null)
        {
            this._href = href;
            this._parent = parent;
            this._title = title;
            this._description = description;
            this._properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets rEQUIRED. URI to the asset object. Relative and absolute URI are both allowed.
        /// </summary>
        /// <value>
        /// REQUIRED. URI to the asset object. Relative and absolute URI are both allowed.
        /// </value>
        [JsonProperty("href")]
        [JsonRequired]
        public string Href { get => this._href; set => this._href = value; }

        /// <summary>
        /// Gets or sets the displayed title for clients and users.
        /// </summary>
        /// <value>
        /// The displayed title for clients and users.
        /// </value>
        [JsonProperty("title")]
        public string Title { get => this._title; set => this._title = value; }

        /// <summary>
        /// Gets or sets a description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </summary>
        /// <value>
        /// A description of the Asset providing additional details, such as how it was processed or created. CommonMark 0.29 syntax MAY be used for rich text representation.
        /// </value>
        [JsonProperty("description")]
        public string Description { get => this._description; set => this._description = value; }

        /// <summary>
        /// Gets or sets additional fields
        /// </summary>
        /// <value>
        /// Additional fields
        /// </value>
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get => this._properties; set => this._properties = value; }

        /// <summary>
        /// Gets parent Stac Object
        /// </summary>
        /// <value>
        /// Parent Stac Object
        /// </value>

        /// <value>
        /// Parent Stac Object
        /// </value>
        [JsonIgnore]
        public IStacObject StacObjectContainer => this._parent;

        /// <summary>
        /// Gets uri
        /// </summary>
        /// <value>
        /// Uri
        /// </value>

        /// <value>
        /// Uri
        /// </value>
        [JsonIgnore]
        public Uri Uri => new Uri(this._href);
    }
}
