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
    public interface IStacObject : IStacPropertiesContainer
    {
        string Id { get; }

        SemVersion StacVersion { get; }

        Collection<StacLink> Links { get; }
        
        ContentType MediaType { get; }

        Collection<string> StacExtensions { get; }
    }
}