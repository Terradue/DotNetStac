// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SimpleLinksCollectionObject.cs

using System.Collections.Generic;

namespace Stac
{
    public class SimpleLinksCollectionObject : ILinksCollectionObject
    {
        public SimpleLinksCollectionObject()
        {
            Links = new List<StacLink>();
        }

        /// <inheritdoc/>
        public ICollection<StacLink> Links { get; set; }
    }
}
