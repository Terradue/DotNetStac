// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SatStacExtensionExtensions.cs

namespace Stac.Extensions.Sat
{
    /// <summary>
    /// Extension methods for the SatStacExtension
    /// </summary>
    public static class SatStacExtensionExtensions
    {
        /// <summary>
        /// Gets the SatStacExtension from the StacItem.
        /// </summary>
        /// <param name="stacItem">The StacItem.</param>
        /// <returns>The SatStacExtension.</returns>
        public static SatStacExtension SatExtension(this StacItem stacItem)
        {
            return new SatStacExtension(stacItem);
        }
    }
}
