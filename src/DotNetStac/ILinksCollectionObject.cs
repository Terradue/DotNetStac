// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ILinksCollectionObject.cs

using System.Collections.Generic;

namespace Stac
{
    public interface ILinksCollectionObject
    {
        ICollection<StacLink> Links { get; }
    }
}
