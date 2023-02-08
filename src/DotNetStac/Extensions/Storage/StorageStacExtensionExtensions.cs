// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StorageStacExtensionExtensions.cs

namespace Stac.Extensions.Storage
{
    /// <summary>
    /// Extension methods for accessing Storage extension
    /// </summary>
    public static class StorageStacExtensionExtensions
    {
        /// <summary>
        /// Get StorageStacExtension class from a STAC Asset
        /// </summary>
        /// <param name="stacAsset">The STAC Asset</param>
        /// <returns>The StorageStacExtension class</returns>
        public static StorageStacExtension StorageExtension(this StacAsset stacAsset)
        {
            return new StorageStacExtension(stacAsset);
        }

        /// <summary>
        /// Get StorageStacExtension class from a STAC Item
        /// </summary>
        /// <param name="stacItem">The STAC Item</param>
        /// <returns>The StorageStacExtension class</returns>
        public static StorageStacExtension StorageExtension(this StacItem stacItem)
        {
            return new StorageStacExtension(stacItem);
        }
    }
}
