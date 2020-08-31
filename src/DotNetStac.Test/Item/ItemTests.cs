using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Item;
using Xunit;

namespace Stac.Test.Item
{
    public class ItemTests : TestBase
    {
        [Fact]
        public void CanDeserializeMinimalSample()
        {
            var json = GetExpectedJson("Item");

            var item = JsonConvert.DeserializeObject<StacItem>(json);

            Assert.NotNull(item);

            Assert.NotNull(item.Properties);

            Assert.Equal("1.0.0-beta.1", item.StacVersion);

            Assert.Empty(item.StacExtensions);

            Assert.Equal(GeoJSONObjectType.Feature, item.Type);

            Assert.Equal("CS3-20160503_132130_04", item.Id);

            Assert.IsType<Polygon>(item.Geometry);

            Assert.Equal(new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 }, item.BoundingBoxes);

            Assert.True(item.Properties.ContainsKey("collection"));
            Assert.Equal("CS3", item.Properties["collection"]);

            Assert.Equal(DateTime.Parse("2016-05-03T13:21:30.040Z").ToUniversalTime(), item.Properties["datetime"]);

            Assert.Contains<StacLink>(item.Links, l => l.RelationshipType == "self" && l.Uri.ToString() == "http://cool-sat.com/catalog/CS3-20160503_132130_04/CS3-20160503_132130_04.json");

            Assert.Contains<StacLink>(item.Links, l => l.RelationshipType == "collection" && l.Uri.ToString() == "http://cool-sat.com/catalog.json");

            Assert.Equal("relative-path/to/analytic.tif", item.Assets["analytic"].Uri.ToString());
            Assert.Equal("4-Band Analytic", item.Assets["analytic"].Title);

            Assert.Equal("http://cool-sat.com/catalog/CS3-20160503_132130_04/thumbnail.png", item.Assets["thumbnail"].Uri.ToString());
            Assert.Equal("Thumbnail", item.Assets["thumbnail"].Title);
            Assert.Contains("thumbnail", item.Assets["thumbnail"].SemanticRoles);
        }

        [Fact]
        public void CanSerializeMinimalSample()
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

            item.BoundingBoxes = new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 };

            var actualJson = JsonConvert.SerializeObject(item);

            Console.WriteLine(actualJson);

            var expectedJson = GetExpectedJson("Item");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public void CanManageDates()
        {
            var json = GetExpectedJson("Item");

            var item = JsonConvert.DeserializeObject<StacItem>(json);

            Assert.Equal(item.DateTime, new Itenso.TimePeriod.TimeInterval(DateTime.Parse("2016-05-03T13:22:30Z").ToUniversalTime()));
        }

        [Fact]
        public void CanDeserializeS2CogSample()
        {
            var json = GetExpectedJson("Item");

            var item = StacItem.LoadJToken(JsonConvert.DeserializeObject<JToken>(json), null);

            Assert.NotNull(item);

            Assert.NotNull(item.Properties);

            Assert.Equal("1.0.0-beta.2", item.StacVersion);

        }
    }
}
