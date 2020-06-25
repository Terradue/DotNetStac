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

namespace Stac.Catalog
{
    public partial class StacCatalog : IStacObject
    {
        private Uri sourceUri;

        public static async Task<StacCatalog> LoadUri(Uri uri)
        {
            WebClient client = new WebClient();

            return await client.DownloadStringTaskAsync(uri).ContinueWith(json => {
                JToken jsonRoot = JsonConvert.DeserializeObject<JToken>(json.Result);
                StacCatalog catalog = null;
                if ( jsonRoot["extent"] != null )
                    catalog = jsonRoot.ToObject<StacCollection>();
                else
                    catalog = jsonRoot.ToObject<StacCatalog>();
                catalog.sourceUri = uri;
                return catalog;
            });
        }

        protected async Task<StacCatalog> LoadUriRelatively(Uri uri)
        {
            if (uri.IsAbsoluteUri)
                return await StacCatalog.LoadUri(uri);
            
            return await StacCatalog.LoadUri(new Uri(new Uri(sourceUri.AbsoluteUri.Substring(0, sourceUri.AbsoluteUri.LastIndexOf('/') + 1 )), uri));
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
