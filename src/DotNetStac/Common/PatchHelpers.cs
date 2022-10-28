using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace Stac.Common
{
    public static class PatchHelpers
    {
        public static T Patch<T>(this T stacObject, IDictionary<string, object> patch) where T : IStacObject
        {
            var itemJson = StacConvert.Serialize(stacObject);
            var patchJson = JsonConvert.SerializeObject(patch);
            var patchedJson = JsonMergeUtils.Merge(itemJson, patchJson, new System.Text.Json.JsonWriterOptions { Indented = false });
            return StacConvert.Deserialize<T>(patchedJson);
        }

        public static T Patch<T>(this T stacObject, IStacObject patch) where T : IStacObject
        {
            var itemJson = StacConvert.Serialize(stacObject);
            var patchJson = StacConvert.Serialize(patch);
            IDictionary<string, object> patchdic = JsonConvert.DeserializeObject<IDictionary<string, object>>(patchJson);
            if (patchdic.ContainsKey("links") && patchdic["links"] is IEnumerable<object>
                && ((IEnumerable<object>)patchdic["links"]).Count() == 0)
            {
                patchdic.Remove("links");
            }
            return Patch(stacObject, patchdic);
        }
    }
}
