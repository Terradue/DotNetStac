using System;
using System.Collections.ObjectModel;
using Stac.Extensions;

namespace Stac
{
    internal interface IInternalStacObject
    {
        string Id { get; }

        string StacVersion { get; set; }

        Uri Uri { get; set; }

        Collection<IStacExtension> StacExtensions { get; set; }

        Collection<StacLink> Links { get; set;  }
    }
}