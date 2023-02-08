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
        /// <summary>
        /// Gets the id of the object
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the title of the object
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the STAC version of the object
        /// </summary>
        SemVersion StacVersion { get; }

        /// <summary>
        /// Gets the description of the object
        /// </summary>
        new ICollection<StacLink> Links { get; }

        /// <summary>
        /// Gets the type of the object
        /// </summary>
        ContentType MediaType { get; }

        /// <summary>
        /// Gets the STAC extensions of the object
        /// </summary>
        ICollection<string> StacExtensions { get; }
    }
}
