using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac
{
    /// <summary>
    /// Common interface for all Stac objects
    /// </summary>
    public interface IStacPropertiesContainer
    {
        IDictionary<string, object> Properties { get; }

        IStacObject StacObjectContainer { get; }

    }
}
