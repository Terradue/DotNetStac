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

        #region Static members

        /// <summary>
        /// Create a new Virtual Asset from assets defined in an existing stac item
        /// </summary>
        /// <param name="stacItem">Stac Item to reference the assets from</param>
        /// <param name="assetsKey">keys of the assets to be referenced</param>
        /// <param name="newStacObject">new stac object container of the virtuals assets. If not specified, the stacItem is used</param>
        /// <returns></returns>
        public static VirtualAsset Create(StacItem stacItem, IList<string> assetsKey, IStacObject newStacObject = null)
        {
            IStacObject parentObject = newStacObject == null ? stacItem : newStacObject;
            return new VirtualAsset(parentObject, stacItem.Assets.Select(asset => new Uri(asset.Value.Uri, "#" + asset.Key)).ToList());
        }

        #endregion

        /// <summary>
        /// Initialize a new Virtual Asset for a STAC object with an array of items
        /// </summary>
        /// <returns></returns>
        public VirtualAsset(IStacObject stacObject, IList<Uri> uris) : base(stacObject, null)
        {
            Uris = uris;
        }

        /// <summary>
        /// array of URIs to the assets object composing the virtual asset. Relative and absolute URI are both allowed
        /// </summary>
        /// <value></value>
        [JsonProperty("href")]
        public IList<Uri> Uris { get; private set; }

        /// <summary>
        /// Do not use
        /// </summary>
        [JsonIgnore]
        public new Uri Uri
        {
            get { return null; }
            set { }
        }

#pragma warning disable 1591
        public bool ShouldSerializeUri() => false;
    }
}
