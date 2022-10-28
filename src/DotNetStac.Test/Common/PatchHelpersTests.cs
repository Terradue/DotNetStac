using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Collection;
using Stac.Common;
using Xunit;

namespace Stac.Test.Item
{
    public class PatchHelpersTests : TestBase
    {
        [Fact]
        public void PatchStacItemTest()
        {
            StacItem baseItem = StacConvert.Deserialize<StacItem>(GetJson("Common", "BaseItem"));

            StacItem patchItem = StacConvert.Deserialize<StacItem>(GetJson("Common", "Patch"));

            StacItem patchedItem = baseItem.Patch<StacItem>(patchItem);

            JsonAssert.AreEqual(GetJson("Common", "PatchedItem"), StacConvert.Serialize(patchedItem));
        }

        [Fact]
        public void PatchStacItemTest2()
        {
            StacItem baseItem = StacConvert.Deserialize<StacItem>(GetJson("Common", "BaseItem"));

            Patch patchItem = JsonConvert.DeserializeObject<Patch>(GetJson("Common", "Patch2"));

            StacItem patchedItem = baseItem.Patch<StacItem>(patchItem);

            JsonAssert.AreEqual(GetJson("Common", "PatchedItem2"), StacConvert.Serialize(patchedItem));
        }
    }
}
