using System;
using System.Collections.ObjectModel;
using Stac.Extensions;

namespace Stac
{
    public interface IStacObject
    {
        string Id { get; }

        string StacVersion { get; }

        Uri Uri { get; }

        Collection<IStacExtension> StacExtensions { get; }

        Collection<StacLink> Links { get; }
    }
}