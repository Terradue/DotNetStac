// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: PatchHelpers.cs

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stac.Common
{
    /// <summary>
    /// Patch helpers
    /// </summary>
    public static class PatchHelpers
    {
        /// <summary>
        /// Patches the specified stac object with a dictionary.
        /// </summary>
        /// <typeparam name="T">The type of the stac object.</typeparam>
        /// <param name="stacObject">The stac object.</param>
        /// <param name="patch">The patch.</param>
        /// <returns>The patched stac object.</returns>
        public static T Patch<T>(this T stacObject, IDictionary<string, object> patch)
            where T : IStacObject
        {
            var itemJson = StacConvert.Serialize(stacObject);
            var patchJson = JsonConvert.SerializeObject(patch);
            var patchedJson = JsonMergeUtils.Merge(itemJson, patchJson, new System.Text.Json.JsonWriterOptions { Indented = false });
            return StacConvert.Deserialize<T>(patchedJson);
        }

        /// <summary>
        /// Patches the specified stac object with another stac object.
        /// </summary>
        /// <typeparam name="T">The type of the stac object.</typeparam>
        /// <param name="stacObject">The stac object.</param>
        /// <param name="patch">The patch.</param>
        /// <returns>The patched stac object.</returns>
        public static T Patch<T>(this T stacObject, IStacObject patch)
            where T : IStacObject
        {
            var itemJson = StacConvert.Serialize(stacObject);
            var patchJson = StacConvert.Serialize(patch);
            IDictionary<string, object> patchdic = JsonConvert.DeserializeObject<IDictionary<string, object>>(patchJson);
            if (patchdic.ContainsKey("links") && patchdic["links"] is IEnumerable<object> en
                && ((IEnumerable<object>)patchdic["links"]).Count() == 0)
            {
                patchdic.Remove("links");
            }

            return Patch(stacObject, patchdic);
        }
    }
}
