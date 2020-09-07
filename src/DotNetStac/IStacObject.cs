using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac
{
    public interface IStacObject
    {
        string Id { get; }

        string StacVersion { get; }

        Uri Uri { get; }

        StacExtensions StacExtensions { get; }

        Collection<StacLink> Links { get; }

        IDictionary<string, object> Properties { get; }

        bool IsCatalog { get; }

        IStacObject Upgrade();
    }
}