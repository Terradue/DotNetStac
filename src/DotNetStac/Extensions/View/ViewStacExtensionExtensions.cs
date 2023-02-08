// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ViewStacExtensionExtensions.cs

namespace Stac.Extensions.View
{
    /// <summary>
    /// Extension methods for accessing View extension
    /// </summary>
    public static class ViewStacExtensionExtensions
    {
        /// <summary>
        /// Gets the View extension from a StacItem
        /// </summary>
        /// <param name="stacItem">The StacItem</param>
        /// <returns>The View extension</returns>
        public static ViewStacExtension ViewExtension(this StacItem stacItem)
        {
            return new ViewStacExtension(stacItem);
        }
    }
}
