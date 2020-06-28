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
using Stac.Model;

namespace Stac.Catalog
{
    public partial class StacCatalog : IStacObject
    {
        private Uri sourceUri;

    

        public static async Task<StacCatalog> LoadUri(Uri uri)
        {
            WebClient client = new WebClient();

            return await client.DownloadStringTaskAsync(uri).ContinueWith<StacCatalog>(json => LoadJsonToken(JsonConvert.DeserializeObject<JToken>(json.Result), uri));
        }

        private static StacCatalog LoadJsonToken(JToken jsonRoot, Uri uri)
        {
            StacCatalog catalog;
            if (jsonRoot["extent"] != null)
                catalog = Stac.Collection.StacCollection.LoadStacCollection(jsonRoot);
            else
                catalog = LoadStacCatalog(jsonRoot);
            catalog.sourceUri = uri;
            return catalog;

        }

        private static StacCatalog LoadStacCatalog(JToken jsonRoot)
        {
            Type catalogType = null;
            if (jsonRoot["stac_version"] == null)
            {
                throw new InvalidDataException("The document is not a STAC document. No 'stac_version' property found");
            }

            try
            {
                catalogType = Stac.Model.SchemaDictionary.GetCatalogTypeFromVersion(jsonRoot["stac_version"].Value<string>());
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException(string.Format("The document has a non supprted version: '{0}'.", jsonRoot["stac_version"].Value<string>()));
            }

            IStacCatalogModelVersion catalog = (IStacCatalogModelVersion)jsonRoot.ToObject(catalogType);

            while (catalog.GetType() != typeof(StacCatalog))
            {
                catalog = catalog.Upgrade();
            }

            return (StacCatalog)catalog;
        }

        protected async Task<StacCatalog> LoadUriRelatively(Uri uri)
        {
            if (uri.IsAbsoluteUri)
                return await StacCatalog.LoadUri(uri);

            return await StacCatalog.LoadUri(new Uri(new Uri(sourceUri.AbsoluteUri.Substring(0, sourceUri.AbsoluteUri.LastIndexOf('/') + 1)), uri));
        }

        public IDictionary<Uri, StacCatalog> GetChildren()
        {
            return GetChildrenAsync().ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Result);
        }

        public IDictionary<Uri, Task<StacCatalog>> GetChildrenAsync()
        {
            return Links.Where(l => l.RelationshipType == "child").ToDictionary(link => link.Uri, link => LoadUriRelatively(link.Uri));
        }
    }
}
