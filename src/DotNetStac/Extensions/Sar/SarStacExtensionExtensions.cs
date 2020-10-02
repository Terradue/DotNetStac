using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Extensions;
using Stac.Item;

namespace Stac.Extensions.Sar
{
    public static class SarStacExtensionExtensions
    {

        public static string[] GetPolarizations(this StacAsset stacAsset)
        {
            string key = Sar.SarStacExtension.Prefix + ":" + Eo.EoStacExtension.BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                return (string[])stacAsset.Properties[key];
            return null;
        }

        public static void SetEoBandObjects(this StacAsset stacAsset, string[] eoBandObjects)
        {
            string key = Sar.SarStacExtension.Prefix + ":" + Eo.EoStacExtension.BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                stacAsset.Properties.Remove(key);
            stacAsset.Properties.Add(key, eoBandObjects);
        }

    }
}
