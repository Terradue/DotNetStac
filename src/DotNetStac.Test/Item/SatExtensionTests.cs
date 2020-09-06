using System;
using System.Collections.Generic;
using System.Linq;
using Stac.Extensions.Sat;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Item;
using Xunit;

namespace Stac.Test.Item
{
    public class SatExtensionTests : TestBase
    {
        [Fact]
        public void CalculatePerpendicularBaseline()
        {
            var sentinel1Json_1 = GetExpectedJson("Item", "S1A_IW_SLC__1SDV_20150305T051937_20150305T052005_004892_006196_ABBB");
            var sentinel1Json_2 = GetExpectedJson("Item", "S1A_IW_SLC__1SDV_20150317T051938_20150317T052005_005067_0065D5_B405");

            IStacItem sentinel1Item_1 = JsonConvert.DeserializeObject<StacItem>(sentinel1Json_1);
            IStacItem sentinel1Item_2 = JsonConvert.DeserializeObject<StacItem>(sentinel1Json_2);

            SatStacExtension satExtension_1 = sentinel1Item_1.GetExtension<SatStacExtension>();
            SatStacExtension satExtension_2 = sentinel1Item_2.GetExtension<SatStacExtension>();

            double baseline = satExtension_1.CalculateBaseline(satExtension_2);


        }

    }
}
