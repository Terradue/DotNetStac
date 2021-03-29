using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Extensions;

namespace Stac.Item
{
    public partial class StacItem : IStacItem, IStacObject
    {

        public static async Task<IStacItem> LoadUri(Uri uri)
        {
            var catalog = await StacFactory.LoadUriAsync(uri);
            if (catalog is IStacItem)
                return (IStacItem)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC item {0}", catalog.Uri));
        }

        public static async Task<IStacItem> LoadStacLink(StacLink link)
        {
            var catalog = await link.LoadAsync();
            if (catalog is IStacItem)
                return (IStacItem)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC item {0}", catalog.Uri));
        }

        public static IStacItem LoadJToken(JToken jsonRoot, Uri uri)
        {
            IStacItem item = LoadStacItem(jsonRoot);
            ((IInternalStacObject)item).Uri = uri;
            return item;
        }

        

        

    }
}
