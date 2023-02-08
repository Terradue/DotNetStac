// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SimpleLinksCollectionObject.cs

using System.Collections.Generic;

namespace Stac
{
    /// <summary>
    /// A simple implementation of <see cref="ILinksCollectionObject"/> that can be used as a base class for other objects.
    /// </summary>
    public class SimpleLinksCollectionObject : ILinksCollectionObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLinksCollectionObject"/> class.
        /// </summary>
        public SimpleLinksCollectionObject()
        {
            this.Links = new List<StacLink>();
        }

        /// <inheritdoc/>
        public ICollection<StacLink> Links { get; set; }
    }
}
