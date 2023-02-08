// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VirtualAssetsStacExtensionExtensions.cs

namespace Stac.Extensions.VirtualAssets
{
    /// <summary>
    /// Extension methods for accessing VirtualAssets extension
    /// </summary>
    public static class VirtualAssetsStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a VirtualAssetsStacExtension class from a STAC item
        /// </summary>
        /// <param name="stacItem">The STAC item</param>
        /// <returns>The VirtualAssetsStacExtension class</returns>
        public static VirtualAssetsStacExtension EoExtension(this StacItem stacItem)
        {
            return new VirtualAssetsStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a VirtualAssetsStacExtension class from a STAC collection
        /// </summary>
        /// <param name="stacCollection">The STAC collection</param>
        /// <returns>The VirtualAssetsStacExtension class</returns>
        public static VirtualAssetsStacExtension EoExtension(this StacCollection stacCollection)
        {
            return new VirtualAssetsStacExtension(stacCollection);
        }
    }
}
