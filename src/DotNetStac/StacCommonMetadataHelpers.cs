using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Catalog;
using Stac.Collection;
using Stac.Extensions;
using Stac.Item;
using Stac.Model;

namespace Stac
{
    public static class StacCommonMetadataHelpers
    {

        public static string GetTitle(this IStacObject stacObject)
        {
            return stacObject.Properties.ContainsKey("title") ? (string)stacObject.Properties["title"] : null;
        }

    }
}
