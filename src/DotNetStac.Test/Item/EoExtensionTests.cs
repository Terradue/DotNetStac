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
using System.IO;
using Stac.Extensions.Eo;

namespace Stac.Test.Item
{
    public class EoExtensionTests : TestBase
    {
        [Fact]
        public void SetAssetBands()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");
            var k3MissingBandsJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_missing_bands");

            IStacItem k3MissingBands = JsonConvert.DeserializeObject<StacItem>(k3MissingBandsJson);

            EoStacExtension k3MissingBandsEO = k3MissingBands.StacExtensions.GetExtension<EoStacExtension>();

            EoBandObject eoBandObject = new EoBandObject("MS1", EoBandCommonName.blue)
            {
                CenterWavelength = 0.485
            };
            eoBandObject.Properties.Add("scale", 27.62430939226519);
            eoBandObject.Properties.Add("offset", -22.1416);
            eoBandObject.Properties.Add("eai", 2001.0);

            Assert.NotNull(k3MissingBands.Assets["MS1"]);

            k3MissingBandsEO.AddBandObject(k3MissingBands.Assets["MS1"], new EoBandObject[] { eoBandObject });

            k3MissingBandsJson = JsonConvert.SerializeObject(k3MissingBands);

            JsonAssert.AreEqual(k3CompleteJson, k3MissingBandsJson);

        }

    }
}
