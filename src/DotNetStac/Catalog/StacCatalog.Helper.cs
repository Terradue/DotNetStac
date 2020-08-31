using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Collection;
using Stac.Item;
using Stac.Model;

namespace Stac.Catalog
{
    public partial class StacCatalog
    {

        public static async Task<IStacCatalog> LoadUri(Uri uri)
        {
            var catalog = await StacFactory.LoadUriAsync(uri);
            if (catalog is IStacCatalog)
                return (IStacCatalog)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC catalog {0}", catalog.Uri));
        }

        public static async Task<IStacCatalog> LoadStacLink(StacLink link)
        {
            var catalog = await StacFactory.LoadStacLink(link);
            if (catalog is IStacCatalog)
                return (IStacCatalog)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC catalog {0}", catalog.Uri));
        }

        public static IStacCatalog LoadJToken(JToken jsonRoot, Uri uri)
        {
            IStacCatalog catalog;
            if (jsonRoot["extent"] != null)
                catalog = Stac.Collection.StacCollection.LoadStacCollection(jsonRoot);
            else
                catalog = LoadStacCatalog(jsonRoot);
            ((IInternalStacObject)catalog).Uri = uri;
            return catalog;

        }

        private static IStacCatalog LoadStacCatalog(JToken jsonRoot)
        {
            Type catalogType = null;
            if (jsonRoot["stac_version"] == null)
            {
                throw new InvalidDataException("The document is not a STAC document. No 'stac_version' property found");
            }
            if (jsonRoot["type"] != null)
            {
                throw new InvalidDataException("The document is not a STAC catalog document. 'type' root property found");
            }

            try
            {
                catalogType = Stac.Model.SchemaDictionary.GetCatalogTypeFromVersion(jsonRoot["stac_version"].Value<string>());
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException(string.Format("The document has a non supprted version: '{0}'.", jsonRoot["stac_version"].Value<string>()));
            }

            return (IStacCatalog)jsonRoot.ToObject(catalogType);
    
        }

    }
}
