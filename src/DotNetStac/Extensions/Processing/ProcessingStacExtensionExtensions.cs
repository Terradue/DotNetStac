// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ProcessingStacExtensionExtensions.cs

namespace Stac.Extensions.Processing
{
    /// <summary>
    /// Helper class to access the <seealso href="https://github.com/stac-extensions/processing">Processing extension</seealso>
    /// </summary>
    public static class ProcessingStacExtensionExtensions
    {
        /// <summary>
        /// Gets the ProcessingStacExtension class from a StacItem
        /// </summary>
        /// <param name="stacItem">The STAC item</param>
        /// <returns>The ProcessingStacExtension class</returns>
        public static ProcessingStacExtension ProcessingExtension(this StacItem stacItem)
        {
            return new ProcessingStacExtension(stacItem);
        }

        /// <summary>
        /// Initialize the major fields of processing extensions
        /// </summary>
        /// <param name="processingStacExtension">The processing extension</param>
        /// <param name="lineage">The lineage</param>
        /// <param name="level">The level</param>
        /// <param name="facility">The facility</param>
        public static void Init(
            this ProcessingStacExtension processingStacExtension,
            string lineage,
            string level,
            string facility = null)
        {
            processingStacExtension.Lineage = lineage;
            processingStacExtension.Level = level;
            processingStacExtension.Facility = facility;
        }
    }
}
