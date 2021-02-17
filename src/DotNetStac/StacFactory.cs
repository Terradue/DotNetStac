using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Collection;
using Stac.Item;
using Stac.Model;

namespace Stac
{
    public class StacFactory
    {
        public static async Task<IStacObject> LoadUriAsync(Uri uri)
        {
            WebClient client = new WebClient();
            return await client.DownloadStringTaskAsync(uri).ContinueWith<IStacObject>(json =>
            {
                var jsonToken = JsonConvert.DeserializeObject<JToken>(json.Result);
                if (jsonToken["geometry"] != null)
                    return StacItem.LoadJToken(jsonToken, uri);

                return StacCollection.LoadJToken(jsonToken, uri);

            });
        }

        public static IStacObject Load(string uri)
        {
            return LoadUriAsync(new Uri(uri)).Result;
        }

    }
}
