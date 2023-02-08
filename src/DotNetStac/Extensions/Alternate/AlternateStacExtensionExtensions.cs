// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: AlternateStacExtension.cs

using Stac.Extensions.Storage;

namespace Stac.Extensions.Alternate
{
    /// <summary>
    /// Extension methods for accessing Alternate extension
    /// </summary>
    public static class AlternateStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a AlternateStacExtension class from a STAC asset
        /// </summary>
        public static AlternateStacExtension AlternateExtension(this StacAsset stacAsset)
        {
            return new AlternateStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a AlternateStacExtension class from an alternate asset
        /// </summary>
        public static StorageStacExtension StorageExtension(this AlternateAssetObject alternateAssetObject)
        {
            return new StorageStacExtension(alternateAssetObject);
        }
    }
}
