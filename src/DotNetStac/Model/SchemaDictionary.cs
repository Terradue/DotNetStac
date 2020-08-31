using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Model
{

    public static class SchemaDictionary
    {
        private static Dictionary<string, Type> catalogVersionDictionary = new Dictionary<string, Type>()
        {
            { "0.6.0", typeof(Stac.Model.v060.StacCatalog060) },
            { "0.7.0", typeof(Stac.Model.v070.StacCatalog070) },
            { "1.0.0-beta.1", typeof(Stac.Catalog.StacCatalog) },
            { "1.0.0-beta.2", typeof(Stac.Catalog.StacCatalog) },
            { "latest", typeof(Stac.Catalog.StacCatalog) }
        };

        private static Dictionary<string, Type> collectionVersionDictionary = new Dictionary<string, Type>()
        {
            { "0.6.0", typeof(Stac.Model.v060.StacCollection060) },
            { "0.7.0", typeof(Stac.Model.v070.StacCollection070) },
            { "1.0.0-beta.1", typeof(Stac.Collection.StacCollection) },
            { "1.0.0-beta.2", typeof(Stac.Collection.StacCollection) },
            { "latest", typeof(Stac.Collection.StacCollection) }
        };

        private static Dictionary<string, Type> itemVersionDictionary = new Dictionary<string, Type>()
        {
            { "0.6.0", typeof(Stac.Model.v060.StacItem060) },
            { "0.7.0", typeof(Stac.Model.v070.StacItem070) },
            { "1.0.0-beta.1", typeof(Stac.Item.StacItem) },
            { "1.0.0-beta.2", typeof(Stac.Item.StacItem) },
            { "latest", typeof(Stac.Item.StacItem) }
        };

        internal static Type GetCatalogTypeFromVersion(string version)
        {
            if (!collectionVersionDictionary.ContainsKey(version))
                return collectionVersionDictionary["latest"];
            return catalogVersionDictionary[version];
        }

        internal static Type GetCollectionTypeFromVersion(string version)
        {
            if (!collectionVersionDictionary.ContainsKey(version))
                return collectionVersionDictionary["latest"];
            return collectionVersionDictionary[version];
            
        }

        internal static Type GetItemTypeFromVersion(string version)
        {
            if (!itemVersionDictionary.ContainsKey(version))
                return itemVersionDictionary["latest"];
            return itemVersionDictionary[version];
        }
    }
}