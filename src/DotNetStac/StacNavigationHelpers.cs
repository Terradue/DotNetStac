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
    public static class StacNavigationHelpers
    {

        public static IDictionary<Uri, IStacCatalog> GetChildren(this IStacObject stacObject)
        {
            return GetChildrenAsync(stacObject).GetAwaiter().GetResult();
        }

        public static async Task<IDictionary<Uri, IStacCatalog>> GetChildrenAsync(this IStacObject stacObject)
        {
            Dictionary<Uri, IStacCatalog> children = new Dictionary<Uri, IStacCatalog>();
            foreach (var link in stacObject.Links.Where(l => l.RelationshipType == "child"))
            {
                children.Add(link.Uri, await StacCatalog.LoadStacLink(link));
            }
            return children;
        }

        public static IDictionary<Uri, IStacItem> GetItems(this IStacObject stacObject)
        {
            return GetItemsAsync(stacObject).GetAwaiter().GetResult();
        }

        public static async Task<IDictionary<Uri, IStacItem>> GetItemsAsync(this IStacObject stacObject)
        {
            Dictionary<Uri, IStacItem> items = new Dictionary<Uri, IStacItem>();
            foreach (var link in stacObject.Links.Where(l => l.RelationshipType == "item"))
            {
                items.Add(link.Uri, await StacItem.LoadStacLink(link));
            }
            return items;
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

        public static T GetExtension<T>(this IStacObject stacObject) where T : IStacExtension
        {
            return (T)stacObject.StacExtensions.Values.FirstOrDefault(e => e is T);
        }
    }
}
