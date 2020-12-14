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

            StacItem k3MissingBands = JsonConvert.DeserializeObject<StacItem>(k3MissingBandsJson);

            EoStacExtension k3MissingBandsEO = k3MissingBands.GetExtension<EoStacExtension>();

            EoBandObject eoBandObject = new EoBandObject("MS1", EoBandCommonName.blue)
            {
                CenterWavelength = 0.485
            };
            eoBandObject.Properties.Add("scale", 27.62430939226519);
            eoBandObject.Properties.Add("offset", -22.1416);
            eoBandObject.Properties.Add("eai", 2001.0);

            Assert.NotNull(k3MissingBands.Assets["MS1"]);

            k3MissingBandsEO.SetAssetBandObjects(k3MissingBands.Assets["MS1"], new EoBandObject[] { eoBandObject });

            k3MissingBandsJson = JsonConvert.SerializeObject(k3MissingBands);

            JsonAssert.AreEqual(k3CompleteJson, k3MissingBandsJson);

            k3MissingBands = JsonConvert.DeserializeObject<StacItem>(k3MissingBandsJson);

            k3MissingBands.Assets["MS1"].SetEoBandObjects(new EoBandObject[] { eoBandObject });

            k3MissingBandsJson = JsonConvert.SerializeObject(k3MissingBands);

            JsonAssert.AreEqual(k3CompleteJson, k3MissingBandsJson);


        }

        [Fact]
        public void GetAssetsBands()
        {
            var k3CompleteJson = GetJson("Item", "GetAssetsBands_K3_20201112193439_45302_18521139_L1G");

            StacItem k3complete = JsonConvert.DeserializeObject<StacItem>(k3CompleteJson);

            EoStacExtension k3EOext = k3complete.GetExtension<EoStacExtension>();

            Assert.NotNull(k3EOext);

            var overviewAssets = k3complete.Assets.Where(a =>
            {
                var bands = k3EOext.GetAssetBandObjects(a.Value);
                if (a.Value.Properties.ContainsKey("eo:bands"))
                {
                    Assert.NotNull(bands);
                    Assert.NotEmpty(bands);
                    return bands.Any(band => !string.IsNullOrEmpty(band.Name));
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
            properties.Add("collection", "CS3");

            StacItem item = new StacItem(geometry, properties, "CS3-20160503_132130_04");

            item.Links.Add(StacLink.CreateSelfLink(new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/CS3-20160503_132130_04.json")));
            item.Links.Add(StacLink.CreateCollectionLink(new Uri("http://cool-sat.com/catalog.json")));

            item.Assets.Add("analytic", new StacAsset(new Uri("relative-path/to/analytic.tif", UriKind.Relative), null, "4-Band Analytic", null));
            item.Assets.Add("thumbnail", StacAsset.CreateThumbnailAsset(new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/thumbnail.png"), null, "Thumbnail"));

            // item.BoundingBoxes = new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 };
            item.BoundingBoxes = item.GetBoundingBoxFromGeometryExtent();

            EoStacExtension eo = new EoStacExtension(item);
            eo.CloudCover = 0;

            var actualJson = JsonConvert.SerializeObject(item);

            var expectedJson = GetJson("Item");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public void CloneEoItem()
        {
            var json = GetJson("Item");

            var item = StacItem.LoadJToken(JsonConvert.DeserializeObject<JToken>(json), null);

            item = new StacItem(item.UpgradeToCurrentVersion());

            var actualJson = JsonConvert.SerializeObject(item);

            JsonAssert.AreEqual(json, actualJson);

            item = StacItem.LoadJToken(JsonConvert.DeserializeObject<JToken>(json), null);

            item = new StacItem(item as StacItem);

            actualJson = JsonConvert.SerializeObject(item);

            JsonAssert.AreEqual(json, actualJson);
        }

    }
}
