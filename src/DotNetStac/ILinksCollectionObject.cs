// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ILinksCollectionObject.cs

using System.Collections.Generic;

namespace Stac
{
    /// <summary>
    /// Interface for objects that have a collection of links
    /// </summary>
    public interface ILinksCollectionObject
    {
        /// <summary>
        /// Gets the links.
        /// </summary>
        ICollection<StacLink> Links { get; }
    }
}
