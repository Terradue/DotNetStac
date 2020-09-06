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
using Stac.Item;
using Stac.Model;

namespace Stac
{
    public static class StacExtensionsHelper
    {

        public static IDictionary<Uri, IStacCatalog> GetChildren(this IStacObject stacObject)
        {
            return GetChildrenAsync(stacObject).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Result);
        }

        public static IDictionary<Uri, Task<IStacCatalog>> GetChildrenAsync(this IStacObject stacObject)
        {
            return stacObject.Links.Where(l => l.RelationshipType == "child").ToDictionary(link => link.Uri, link => StacCatalog.LoadStacLink(link));
        }

        public static IDictionary<Uri, IStacItem> GetItems(this IStacObject stacObject)
        {
            return GetItemsAsync(stacObject).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Result);
        }

        public static IDictionary<Uri, Task<IStacItem>> GetItemsAsync(this IStacObject stacObject)
        {
            return stacObject.Links.Where(l => l.RelationshipType == "item").ToDictionary(link => link.Uri, link => StacItem.LoadStacLink(link));
        }

        public static IEnumerable<StacLink> GetChildrenLinks(this IStacObject stacObject, bool absolute = true)
        {
            return stacObject.Links.Where(l => l.RelationshipType == "child");
        }

        public static IEnumerable<StacLink> GetItemLinks(this IStacObject stacObject, bool absolute = true)
        {
            return stacObject.Links.Where(l => l.RelationshipType == "item");
        }

         public static StacItem UpgradeToCurrentVersion(this IStacItem item1)
        {
            IStacObject item = (IStacObject)item1;
            while (!(item is Item.StacItem))
            {
                item = item.Upgrade();
            }
            return (Item.StacItem)item;
        }

         public static StacCatalog UpgradeToCurrentVersion(this IStacCatalog catalog1)
        {
            IStacObject catalog = (IStacObject)catalog1;
            while (!(catalog is StacCatalog))
            {
                catalog = catalog.Upgrade();
            }
            return (StacCatalog)catalog;
        }
    }
}
