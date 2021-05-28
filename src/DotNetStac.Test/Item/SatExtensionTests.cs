using Newtonsoft.Json;
using Stac.Extensions.Sat;
using Xunit;

namespace Stac.Test.Item
{
    public class SatExtensionTests : TestBase
    {
        [Fact]
        public void CalculatePerpendicularBaseline()
        {
            var sentinel1Json_1 = GetJson("Item", "S1A_IW_SLC__1SDV_20150305T051937_20150305T052005_004892_006196_ABBB");
            var sentinel1Json_2 = GetJson("Item", "S1A_IW_SLC__1SDV_20150317T051938_20150317T052005_005067_0065D5_B405");

            StacItem sentinel1Item_1 = StacConvert.Deserialize<StacItem>(sentinel1Json_1);
            StacItem sentinel1Item_2 = StacConvert.Deserialize<StacItem>(sentinel1Json_2);

            var baseline = sentinel1Item_1.SatExtension().CalculateBaseline(sentinel1Item_2.SatExtension());

        }

    }
}
