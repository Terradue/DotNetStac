// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VirtualAsset.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stac.Extensions.VirtualAssets
{
    /// <summary>
    /// The virtual asset object is an extension of the core asset object for the Virtual Assets extension
    /// </summary>
    public class VirtualAsset : StacAsset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualAsset"/> class.
        /// </summary>
        /// <param name="stacObject">The stac object.</param>
        /// <param name="uris">The uris.</param>
        public VirtualAsset(IStacObject stacObject, IList<Uri> uris)
            : base(stacObject, null)
        {
            this.Uris = uris;
        }

        /// <summary>
        /// Gets array of URIs to the assets object composing the virtual asset. Relative and absolute URI are both allowed
        /// </summary>
        [JsonProperty("href")]
        public IList<Uri> Uris { get; private set; }

        /// <summary>
        /// Gets or sets do not use
        /// </summary>
        [JsonIgnore]
        public new Uri Uri
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Create a new Virtual Asset from assets defined in an existing stac item
        /// </summary>
        /// <param name="stacItem">Stac Item to reference the assets from</param>
        /// <param name="assetsKey">keys of the assets to be referenced</param>
        /// <param name="newStacObject">new stac object container of the virtuals assets. If not specified, the stacItem is used</param>
        /// <returns>Virtual Asset</returns>
        public static VirtualAsset Create(StacItem stacItem, IList<string> assetsKey, IStacObject newStacObject = null)
        {
            IStacObject parentObject = newStacObject ?? stacItem;
            return new VirtualAsset(parentObject, stacItem.Assets.Select(asset => new Uri(asset.Value.Uri, "#" + asset.Key)).ToList());
        }

#pragma warning disable 1591
        public bool ShouldSerializeUri() => false;
#pragma warning restore 1591
    }
}
