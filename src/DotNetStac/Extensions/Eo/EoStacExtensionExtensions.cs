// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: EoStacExtensionExtensions.cs

using System.Linq;

namespace Stac.Extensions.Eo
{
    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class EoStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        /// <param name="stacItem">Stac Item</param>
        /// <returns>EoStacExtension class</returns>
        public static EoStacExtension EoExtension(this StacItem stacItem)
        {
            return new EoStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a EoStacExtension class from a STAC asset
        /// </summary>
        /// <param name="stacAsset">Stac Asset</param>
        /// <returns>EoStacExtension class</returns>
        public static EoStacExtension EoExtension(this StacAsset stacAsset)
        {
            return new EoStacExtension(stacAsset);
        }

        /// <summary>
        /// Get a STAC asset from a STAC item by its common name
        /// </summary>
        /// <param name="stacItem">Stac Item</param>
        /// <param name="commonName">common name</param>
        /// <returns>Stac Asset</returns>
        public static StacAsset GetAsset(this StacItem stacItem, EoBandCommonName commonName)
        {
            return stacItem.Assets.Values.Where(a => a.EoExtension().Bands != null).FirstOrDefault(a => a.EoExtension().Bands.Any(b => b.CommonName == commonName));
        }

        /// <summary>
        /// Get a STAC EO Band object from a STAC item by its common name
        /// </summary>
        /// <param name="stacItem">Stac Item</param>
        /// <param name="commonName">common name</param>
        /// <returns>Stac EO Band object</returns>
        public static EoBandObject GetBandObject(this StacItem stacItem, EoBandCommonName commonName)
        {
            return stacItem.Assets.Values.Where(a => a.EoExtension().Bands != null).Select(a => a.EoExtension().Bands.FirstOrDefault(b => b.CommonName == commonName)).First();
        }
    }
}
