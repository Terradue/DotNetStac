﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VersionStacExtensionExtensions.cs

using System;
using System.Linq;

namespace Stac.Extensions.Version
{
    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class VersionStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a VersionStacExtension class from a STAC item
        /// </summary>
        /// <param name="stacItem">The STAC item</param>
        /// <returns>The VersionStacExtension class</returns>
        public static VersionStacExtension VersionExtension(this StacItem stacItem)
        {
            return new VersionStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a VersionStacExtension class from a STAC collection
        /// </summary>
        /// <param name="stacCollection">The STAC collection</param>
        /// <returns>The VersionStacExtension class</returns>
        public static VersionStacExtension VersionExtension(this StacCollection stacCollection)
        {
            return new VersionStacExtension(stacCollection);
        }

        /// <summary>
        /// Retrieve the predecessor version of the Stac Item if any
        /// </summary>
        /// <param name="stacItem">current Stac Item</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no precedessor version</returns>
        public static StacItem PredecessorVersion(this StacItem stacItem, Func<StacLink, StacItem> stacLinkResolver)
        {
            return GetVersion(stacItem, VersionStacExtension.PredecessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the predecessor version of the Stac Collection if any
        /// </summary>
        /// <param name="stacCollection">current Stac Collection</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no precedessor version</returns>
        public static StacCollection PredecessorVersion(this StacCollection stacCollection, Func<StacLink, StacCollection> stacLinkResolver)
        {
            return GetVersion(stacCollection, VersionStacExtension.PredecessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the successor version of the Stac Item if any
        /// </summary>
        /// <param name="stacItem">current Stac Item</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no successor version</returns>
        public static StacItem SuccessorVersion(this StacItem stacItem, Func<StacLink, StacItem> stacLinkResolver)
        {
            return GetVersion(stacItem, VersionStacExtension.SuccessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the successor version of the Stac Collection if any
        /// </summary>
        /// <param name="stacCollection">current Stac Collection</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no successor version</returns>
        public static StacCollection SuccessorVersion(this StacCollection stacCollection, Func<StacLink, StacCollection> stacLinkResolver)
        {
            return GetVersion(stacCollection, VersionStacExtension.SuccessorVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the latest version of the Stac Item if any
        /// </summary>
        /// <param name="stacItem">current Stac Item</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no latest version</returns>
        public static StacItem LatestVersion(this StacItem stacItem, Func<StacLink, StacItem> stacLinkResolver)
        {
            return GetVersion(stacItem, VersionStacExtension.LatestVersionRel, stacLinkResolver);
        }

        /// <summary>
        /// Retrieve the latest version of the Stac Collection if any
        /// </summary>
        /// <param name="stacCollection">current Stac Collection</param>
        /// <param name="stacLinkResolver">Function that read a Stac Link to get the StacItem</param>
        /// <returns>null if no latest version</returns>
        public static StacCollection LatestVersion(this StacCollection stacCollection, Func<StacLink, StacCollection> stacLinkResolver)
        {
            return GetVersion(stacCollection, VersionStacExtension.LatestVersionRel, stacLinkResolver);
        }

        internal static T GetVersion<T>(this T stacObject, string relType, Func<StacLink, T> stacLinkResolver)
            where T : IStacObject
        {
            var predecessorVersionLink = stacObject.Links.FirstOrDefault(l => l.RelationshipType == relType);
            if (predecessorVersionLink == null)
            {
                return default(T);
            }

            return stacLinkResolver(predecessorVersionLink);
        }
    }
}
