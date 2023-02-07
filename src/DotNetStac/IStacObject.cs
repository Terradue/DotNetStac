// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: IStacObject.cs

using System.Collections.Generic;
using System.Net.Mime;
using Semver;

namespace Stac
{
    /// <summary>
    /// Common interface for all Stac objects
    /// </summary>
    public interface IStacObject : IStacPropertiesContainer, ILinksCollectionObject
    {
        string Id { get; }

        string Title { get; }

        SemVersion StacVersion { get; }

        new ICollection<StacLink> Links { get; }

        ContentType MediaType { get; }

        ICollection<string> StacExtensions { get; }
    }
}
