using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Xunit;
using Stac.Extensions.Sar;
using System;

namespace Stac.Test.Item
{
    public class SarExtensionTests : TestBase
    {

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

            StacItem stacItem = new StacItem("S1A_IW_GRDH_1SDV_20160822T182823_20160822T182848_012717_013FFE_90AF", geometry, properties);

            stacItem.CommonMetadata().DateTime = new Itenso.TimePeriod.TimeInterval(DateTime.Parse("2016-08-22T18:28:23.368922Z"));
            stacItem.CommonMetadata().Platform = "sentinel-1a";

            stacItem.SarExtension().Required("IW", SarCommonFrequencyBandName.C, new string[2] { "VV", "VH" }, "GRD");

            var actualJson = StacConvert.Serialize(stacItem);

            ValidateJson(actualJson);

            var expectedJson = GetJson("Item", "S1A_IW_GRDH_1SDV_20160822T182823_20160822T182848_012717_013FFE_90AF");

            ValidateJson(expectedJson);

            JsonAssert.AreEqual(expectedJson, actualJson);

        }

    }
}
