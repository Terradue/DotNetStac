// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ProjectionStacExtensionExtensions.cs

namespace Stac.Extensions.Projection
{
    /// <summary>
    /// Helper class to access the Projection extension.
    /// </summary>
    public static class ProjectionStacExtensionExtensions
    {
        /// <summary>
        /// Get the Projection extension from a StacItem.
        /// </summary>
        /// <param name="stacItem">The StacItem.</param>
        /// <returns>The Projection extension.</returns>
        public static ProjectionStacExtension ProjectionExtension(this StacItem stacItem)
        {
            return new ProjectionStacExtension(stacItem);
        }

        /// <summary>
        /// Get the Projection extension from a Stac Asset.
        /// </summary>
        /// <param name="stacAsset">The Stac Asset.</param>
        /// <returns>The Projection extension.</returns>
        public static ProjectionStacExtension ProjectionExtension(this StacAsset stacAsset)
        {
            return new ProjectionStacExtension(stacAsset);
        }
    }
}
