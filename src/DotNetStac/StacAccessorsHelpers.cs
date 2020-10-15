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
    public static class StacAccessorsHelpers
    {

        public static void SetProperty(this IStacObject stacObject, string key, object value)
        {
            stacObject.Properties.Remove(key);
            stacObject.Properties.Add(key, value);
        }

        public static T GetProperty<T>(this IStacObject stacObject, string key)
        {
            if (!stacObject.Properties.ContainsKey(key))
                return default(T);
            return (T)stacObject.Properties[key];
        }

    }
}
