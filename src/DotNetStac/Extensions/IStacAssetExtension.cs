// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: IStacAssetExtension.cs

namespace Stac.Extensions
{
    /// <summary>
    /// Interface for StacAsset extensions
    /// </summary>
    public interface IStacAssetExtension
    {
        /// <summary>
        /// Gets the StacAsset.
        /// </summary>
        StacAsset StacAsset { get; }
    }
}
