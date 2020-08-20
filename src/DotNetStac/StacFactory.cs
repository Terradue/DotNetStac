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

        public static async Task<IStacObject> LoadStacLink(StacLink link)
        {
            WebClient client = new WebClient();
            var jsonToken = JsonConvert.DeserializeObject<JToken>(await client.DownloadStringTaskAsync(link.AbsoluteUri));
            if (jsonToken["stac_version"] == null && link.Parent != null && Version.Parse(link.Parent.StacVersion).CompareTo(Version.Parse("0.8.0")) < 0)
                jsonToken["stac_version"] = link.Parent.StacVersion;

            if (jsonToken["geometry"] != null)
                return StacItem.LoadJToken(jsonToken, link.AbsoluteUri);

            return StacCollection.LoadJToken(jsonToken, link.AbsoluteUri);
        }

    }
}
