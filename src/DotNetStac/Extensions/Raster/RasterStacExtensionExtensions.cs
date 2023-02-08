// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: RasterStacExtensionExtensions.cs

namespace Stac.Extensions.Raster
{
    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class RasterStacExtensionExtensions
    {
        /// <summary>
        /// Gets RasterStacExtension class from a STAC asset
        /// </summary>
        /// <param name="stacAsset">The STAC asset</param>
        /// <returns>The RasterStacExtension class</returns>
        public static RasterStacExtension RasterExtension(this StacAsset stacAsset)
        {
            return new RasterStacExtension(stacAsset);
        }
    }
}
