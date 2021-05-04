using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Stac.Extensions.Eo;

namespace Stac.Test.Extensions
{
    public class EoExtensionTests : TestBase
    {
        [Fact]
        public void SetAssetBands()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");
            var k3MissingBandsJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_missing_bands");

            ValidateJson(k3CompleteJson);
            ValidateJson(k3MissingBandsJson);

            StacItem k3MissingBands = StacConvert.Deserialize<StacItem>(k3MissingBandsJson);

            EoBandObject eoBandObject = new EoBandObject("MS1", EoBandCommonName.blue)
            {
                CenterWavelength = 0.485
            };
            eoBandObject.Properties.Add("scale", 27.62430939226519);
            eoBandObject.Properties.Add("offset", -22.1416);
            eoBandObject.Properties.Add("eai", 2001.0);

            Assert.NotNull(k3MissingBands.Assets["MS1"]);

            k3MissingBands.Assets["MS1"].EoExtension().Bands = new EoBandObject[] { eoBandObject };

            Assert.NotNull(k3MissingBands.GetAsset(EoBandCommonName.blue));

            k3MissingBandsJson = JsonConvert.SerializeObject(k3MissingBands);

            ValidateJson(k3MissingBandsJson);

            JsonAssert.AreEqual(k3CompleteJson, k3MissingBandsJson);
        }

        [Fact]
        public void GetAssetsBands()
        {
            var k3CompleteJson = GetJson("Item", "GetAssetsBands_K3_20201112193439_45302_18521139_L1G");

            ValidateJson(k3CompleteJson);

            StacItem k3complete = StacConvert.Deserialize<StacItem>(k3CompleteJson);

            var overviewAssets = k3complete.Assets.Where(a =>
            {
                if (a.Value.Properties.ContainsKey("eo:bands"))
                {
                    Assert.NotNull(k3complete.EoExtension().Bands);
                    Assert.NotEmpty(k3complete.EoExtension().Bands);
                    return k3complete.EoExtension().Bands.Any(band => !string.IsNullOrEmpty(band.Name));
                }
                return false;
            });

            Assert.Equal(5, overviewAssets.Count());
        }

        [Fact]
        public void CreateEoExtension()
        {
            var coordinates = new[]
           {
                new List<IPosition>
                {
                    new Position(37.488035566,-122.308150179),
                    new Position(37.538869539,-122.597502109),
                    new Position(37.613537207,-122.576687533),
                    new Position(37.562818007,-122.288048600),
                    new Position(37.488035566,-122.308150179)
                }
            };

            var geometry = new Polygon(new LineString[] { new LineString(coordinates[0]) });

            var properties = new Dictionary<string, object>();

            properties.Add("datetime", DateTime.Parse("2016-05-03T13:21:30.040Z").ToUniversalTime());

            StacItem item = new StacItem("CS3-20160503_132130_04", geometry, properties);

            item.Links.Add(StacLink.CreateSelfLink(new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/CS3-20160503_132130_04.json")));
            item.SetCollection("CS3", new Uri("http://cool-sat.com/catalog.json"));

            item.Assets.Add("analytic", new StacAsset(item, new Uri("relative-path/to/analytic.tif", UriKind.Relative), null, "4-Band Analytic", null));
            item.Assets.Add("thumbnail", StacAsset.CreateThumbnailAsset(item, new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/thumbnail.png"), null, "Thumbnail"));

            // item.BoundingBoxes = new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 };
            item.BoundingBoxes = item.GetBoundingBoxFromGeometryExtent();

            EoStacExtension eo = new EoStacExtension(item);
            eo.CloudCover = 0;

            Assert.Equal<double>(0, eo.CloudCover);

            var actualJson = StacConvert.Serialize(item);

            ValidateJson(actualJson);

            var expectedJson = GetJson("Item");

            ValidateJson(expectedJson);

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public void CloneEoItem()
        {
            var json = GetJson("Item");

            ValidateJson(json);

            var item = StacConvert.Deserialize<StacItem>(json);

            var actualJson = StacConvert.Serialize(item);

            ValidateJson(actualJson);

            JsonAssert.AreEqual(json, actualJson);

            item = item = StacConvert.Deserialize<StacItem>(json);

            item = new StacItem(item as StacItem);

            actualJson = JsonConvert.SerializeObject(item);

            JsonAssert.AreEqual(json, actualJson);
        }

    }
}
