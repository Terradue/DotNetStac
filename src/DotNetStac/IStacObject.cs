using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac
{
    /// <summary>
    /// Common interface for all Stac objects
    /// </summary>
    public interface IStacObject : IStacPropertiesContainer
    {
        string Id { get; }

        string StacVersion { get; }

        Uri Uri { get; }

        StacExtensions StacExtensions { get; }

        Collection<StacLink> Links { get; }

        bool IsCatalog { get; }
        
        ContentType MediaType { get; }

        IStacObject Upgrade();
    }
}