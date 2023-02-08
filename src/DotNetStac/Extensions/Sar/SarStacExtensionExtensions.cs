// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SarStacExtensionExtensions.cs

using System;
using System.Linq;

namespace Stac.Extensions.Sar
{
    /// <summary>
    /// Helper class to access the SAR extension.
    /// </summary>
    public static class SarStacExtensionExtensions
    {
        /// <summary>
        /// Gets the SAR extension.
        /// </summary>
        /// <param name="stacItem">The STAC item.</param>
        /// <returns>The SAR extension.</returns>
        public static SarStacExtension SarExtension(this StacItem stacItem)
        {
            return new SarStacExtension(stacItem);
        }

        /// <summary>
        /// Gets the SAR extension.
        /// </summary>
        /// <param name="stacAsset">The STAC asset.</param>
        /// <returns>The SAR extension.</returns>
        public static SarStacExtension SarExtension(this StacAsset stacAsset)
        {
            return new SarStacExtension(stacAsset);
        }

        /// <summary>
        /// Gets the Stac Asset with the specified polarization.
        /// </summary>
        /// <param name="stacItem">The STAC item.</param>
        /// <param name="polarization">The polarization.</param>
        /// <returns>The Stac Asset.</returns>
        public static StacAsset GetAsset(this StacItem stacItem, string polarization)
        {
            return stacItem.Assets.Values.FirstOrDefault(a => a.SarExtension().Polarizations.Contains(polarization));
        }

        /// <summary>
        /// Sets the required SAR extension properties.
        /// </summary>
        /// <param name="sarStacExtension">The SAR extension.</param>
        /// <param name="instrumentMode">The instrument mode.</param>
        /// <param name="frequencyBandName">Name of the frequency band.</param>
        /// <param name="polarizations">The polarizations.</param>
        /// <param name="productType">Type of the product.</param>
        public static void Required(
            this SarStacExtension sarStacExtension,
            string instrumentMode,
            SarCommonFrequencyBandName frequencyBandName,
            string[] polarizations,
            string productType)
        {
            sarStacExtension.InstrumentMode = instrumentMode;
            sarStacExtension.FrequencyBand = frequencyBandName;
            sarStacExtension.Polarizations = polarizations;
            sarStacExtension.ProductType = productType;
        }
    }
}
