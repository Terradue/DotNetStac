using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac
{
    /// <summary>
    /// Common interface for all Stac objects that can have children links
    /// </summary>
    public interface IStacParent : IStacObject
    {
        
    }
}