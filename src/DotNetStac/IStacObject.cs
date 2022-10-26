using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using Semver;
using Stac.Extensions;

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

        ICollection<StacLink> Links { get; }

        ContentType MediaType { get; }

        ICollection<string> StacExtensions { get; }

    }
}
