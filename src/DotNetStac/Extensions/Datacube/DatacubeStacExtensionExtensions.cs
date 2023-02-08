// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeStacExtensionExtensions.cs

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class DatacubeStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a DatacubeStacExtension class from a STAC asset
        /// </summary>
        /// <param name="stacAsset">The STAC asset.</param>
        /// <returns>The DatacubeStacExtension class</returns>
        public static DatacubeStacExtension DatacubeStacExtension(this StacAsset stacAsset)
        {
            return new DatacubeStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a DatacubeStacExtension class from a STAC item
        /// </summary>
        /// <param name="stacItem">The STAC item.</param>
        /// <returns>The DatacubeStacExtension class</returns>
        public static DatacubeStacExtension DatacubeStacExtension(this StacItem stacItem)
        {
            return new DatacubeStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a DatacubeStacExtension class from a STAC collection
        /// </summary>
        /// <param name="stacCollection">The STAC collection.</param>
        /// <returns>The DatacubeStacExtension class</returns>
        public static DatacubeStacExtension DatacubeStacExtension(this StacCollection stacCollection)
        {
            return new DatacubeStacExtension(stacCollection);
        }
    }
}
