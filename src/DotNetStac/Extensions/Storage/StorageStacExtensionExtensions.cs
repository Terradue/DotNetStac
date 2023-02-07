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
        /// Initilize a StorageStacExtension class from a STAC Asset
        /// </summary>
        /// <param name="stacAsset">The STAC Asset</param>
        public static StorageStacExtension StorageExtension(this StacAsset stacAsset)
        {
            return new StorageStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a StorageStacExtension class from a STAC Item
        /// </summary>
        /// <param name="stacItem">The STAC Item</param>
        public static StorageStacExtension StorageExtension(this StacItem stacItem)
        {
            return new StorageStacExtension(stacItem);
        }
    }
}
