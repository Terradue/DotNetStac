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
using Stac.Extensions.Sar;
using Stac.Extensions;

namespace Stac.Test.Item
{
    public class SarExtensionTests : TestBase
    {
        [Fact]
        public void CreateSatExtensionWithNullItem()
        {
            SarStacExtension sar = null;
            Assert.Throws(typeof(ExtensionNotAssignedException),
                () => sar = new SarStacExtension(null, "IW", SarCommonFrequencyBandName.C, new string[2] { "VV", "VH" }, "GRD"));

            Assert.Null(sar);
        }

        [Fact]
        public void TestFullSar()
        {

            var coordinates = new[]
            {
                new List<IPosition>
                {
                    new Position(14.953436,-5.730959),
                    new Position(15.388663,-3.431006),
                    new Position(13.880572,-3.136116),
                    new Position(13.441674,-5.419919),
                    new Position(14.953436,-5.730959)
                }
            };

            var geometry = new Polygon(new LineString[] { new LineString(coordinates[0]) });

            var properties = new Dictionary<string, object>();

            properties.Add("datetime", "2016-08-22T18:28:23.368922Z");
            properties.Add("platform", "sentinel-1a");

            StacItem stacItem = new StacItem(geometry, properties, "S1A_IW_GRDH_1SDV_20160822T182823_20160822T182848_012717_013FFE_90AF");

            SarStacExtension sar = new SarStacExtension(stacItem, "IW", SarCommonFrequencyBandName.C, new string[2] { "VV", "VH" }, "GRD");

            var actualJson = JsonConvert.SerializeObject(stacItem);

            var expectedJson = GetJson("Item", "S1A_IW_GRDH_1SDV_20160822T182823_20160822T182848_012717_013FFE_90AF");

            JsonAssert.AreEqual(expectedJson, actualJson);

        }

    }
}
