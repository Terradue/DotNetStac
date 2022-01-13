using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Exceptions;
using Xunit;

namespace Stac.Test.Item
{
    public class ItemTests : TestBase
    {
        [Fact]
        public void CanDeserializeMinimalSample()
        {
            var json = GetJson("Item");

            ValidateJson(json);

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.NotNull(item);

            Assert.NotNull(item.Properties);

            Assert.Equal("1.0.0", item.StacVersion);

            Assert.Empty(item.StacExtensions);

            Assert.Equal(GeoJSONObjectType.Feature, item.Type);

            Assert.Equal("CS3-20160503_132130_04", item.Id);

            Assert.IsType<Polygon>(item.Geometry);

            Assert.Equal(new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 }, item.BoundingBoxes);

            Assert.Equal("CS3", item.Collection);

            Assert.Equal(DateTime.Parse("2016-05-03T13:21:30.040Z").ToUniversalTime(), item.Properties["datetime"]);

            Assert.Contains<StacLink>(item.Links, l => l.RelationshipType == "self" && l.Uri.ToString() == "http://cool-sat.com/catalog/CS3-20160503_132130_04/CS3-20160503_132130_04.json");

            Assert.Contains<StacLink>(item.Links, l => l.RelationshipType == "collection" && l.Uri.ToString() == "http://cool-sat.com/catalog.json");

            Assert.Equal("relative-path/to/analytic.tif", item.Assets["analytic"].Uri.ToString());
            Assert.Equal("4-Band Analytic", item.Assets["analytic"].Title);

            Assert.Equal("http://cool-sat.com/catalog/CS3-20160503_132130_04/thumbnail.png", item.Assets["thumbnail"].Uri.ToString());
            Assert.Equal("Thumbnail", item.Assets["thumbnail"].Title);
            Assert.Contains("thumbnail", item.Assets["thumbnail"].Roles);
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

            properties.Add("collection", "CS3");

            StacItem item = new StacItem("CS3-20160503_132130_04", geometry, properties);

            item.DateTime = new Itenso.TimePeriod.TimeInterval(DateTime.Parse("2016-05-03T13:21:30.040Z"));

            item.Links.Add(StacLink.CreateSelfLink(new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/CS3-20160503_132130_04.json")));
            item.SetCollection("cool-sat", new Uri("http://cool-sat.com/catalog.json"));

            item.Assets.Add("analytic", new StacAsset(item, new Uri("relative-path/to/analytic.tif", UriKind.Relative), null, "4-Band Analytic", null));
            item.Assets.Add("thumbnail", StacAsset.CreateThumbnailAsset(item, new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/thumbnail.png"), null, "Thumbnail"));

            // item.BoundingBoxes = new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 };
            item.BoundingBoxes = item.GetBoundingBoxFromGeometryExtent();

            var actualJson = StacConvert.Serialize(item);

            ValidateJson(actualJson);

            var expectedJson = GetJson("Item");

            ValidateJson(expectedJson);

            JsonAssert.AreEqual(expectedJson, actualJson);

            item.Links.Remove(item.Links.First(l => l.RelationshipType == "collection"));
            Assert.Null(item.Collection);
        }

        [Fact]
        public void CanSerializeExtendedSample()
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

            properties.Add("collection", "CS3");

            StacItem item = new StacItem("CS3-20160503_132130_04", geometry, properties);

            item.DateTime = new Itenso.TimePeriod.TimeInterval(DateTime.MinValue, DateTime.MaxValue);
            item.DateTime = new Itenso.TimePeriod.TimeInterval(DateTime.Parse("2016-05-03T13:21:30.040Z"), DateTime.Parse("2016-05-03T14:21:30.040Z"));

            item.Links.Add(StacLink.CreateSelfLink(new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/CS3-20160503_132130_04.json")));
            item.SetCollection("cool-sat", new Uri("http://cool-sat.com/catalog.json"));

            item.Assets.Add("analytic", new StacAsset(item, new Uri("relative-path/to/analytic.tif", UriKind.Relative), null, "4-Band Analytic", null));
            item.Assets.Add("thumbnail", StacAsset.CreateThumbnailAsset(item, new Uri("http://cool-sat.com/catalog/CS3-20160503_132130_04/thumbnail.png"), null, "Thumbnail"));

            item.Created = new DateTime(2018, 1, 1);
            item.Updated = new DateTime(2018, 1, 1);

            Assert.Equal(new DateTime(2018, 1, 1), item.Created);
            Assert.Equal(new DateTime(2018, 1, 1), item.Updated);

            item.Gsd = 0;
            item.Gsd = 1;
            Assert.Equal(1, item.Gsd);

            item.Title = "CS3-20160503_132130_04";
            Assert.Equal("CS3-20160503_132130_04", item.Title);

            item.Platform = "coolsat-3";
            Assert.Equal("coolsat-3", item.Platform);

            item.Mission = "coolsat-3";
            Assert.Equal("coolsat-3", item.Mission);

            item.Constellation = "coolsat";
            Assert.Equal("coolsat", item.Constellation);

            item.Instruments = new string[] { "coolins" };
            Assert.Equal(new string[] { "coolins" }, item.Instruments);

            // item.BoundingBoxes = new double[4] { -122.59750209, 37.48803556, -122.2880486, 37.613537207 };
            item.BoundingBoxes = item.GetBoundingBoxFromGeometryExtent();

            var actualJson = StacConvert.Serialize(item);

            ValidateJson(actualJson);

            var expectedJson = GetJson("Item");

            ValidateJson(expectedJson);

            JsonAssert.AreEqual(expectedJson, actualJson);

            item.Links.Remove(item.Links.First(l => l.RelationshipType == "collection"));
            Assert.Null(item.Collection);
        }

        [Fact]
        public void CanManageDates()
        {
            var json = GetJson("Item");

            ValidateJson(json);

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.Equal(item.DateTime, new Itenso.TimePeriod.TimeInterval(DateTime.Parse("2016-05-03T13:22:30Z").ToUniversalTime()));
        }

        [Fact]
        public void CanDeserializeS2CogSample()
        {
            var json = GetJson("Item");

            ValidateJson(json);

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.NotNull(item);

            Assert.NotNull(item.Properties);

            Assert.Equal("1.0.0", item.StacVersion);

        }

        [Fact]
        public void CannotMakeEmptyGeometryItem()
        {
            Assert.Throws<ArgumentNullException>(() => new StacItem(null));
        }

        [Fact]
        public void GetProperty()
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

            StacItem item = new StacItem("CS3-20160503_132130_04", geometry, properties);

            item.SetProperty("test", new string[] { "test1", "test2", "test3" });

            string json = StacConvert.Serialize(item);

            ValidateJson(json);

            item = StacConvert.Deserialize<StacItem>(json);

            var array = item.GetProperty<string[]>("test");
        }

        [Fact]
        public void Geometry()
        {
            var pextentCheck = new[]
            {
                new List<IPosition>
                {
                    new Position(37.488035566,-122.308150179, 10),
                    new Position(37.488035566,-122.308150179, 10),
                }
            };

            var extentCheck = new[]
            {
                new List<IPosition>
                {
                    new Position(37.488035566,-122.597502109, 10),
                    new Position(37.613537207,-122.288048600, 10),
                }
            };

            var coordinates = new[]
            {
                new List<IPosition>
                {
                    new Position(37.488035566,-122.308150179, 10),
                    new Position(37.538869539,-122.597502109, 10),
                    new Position(37.613537207,-122.576687533, 10),
                    new Position(37.562818007,-122.288048600, 10),
                    new Position(37.488035566,-122.308150179, 10)
                }
            };

            Point point = new Point(coordinates[0][0]);
            var extent = StacGeometryHelpers.GetBoundingBox(point);
            Assert.Equal<IPosition>(pextentCheck.First().ToArray(), extent);
            MultiPoint mpoint = new MultiPoint(Array.ConvertAll<IPosition, Point>(coordinates[0].ToArray(), p => new Point(p)));
            extent = StacGeometryHelpers.GetBoundingBox(mpoint);
            Assert.Equal<IPosition>(extentCheck.First().ToArray(), extent);
            var lineString = new LineString(coordinates[0]);
            extent = StacGeometryHelpers.GetBoundingBox(lineString);
            Assert.Equal<IPosition>(extentCheck.First().ToArray(), extent);
            var mlinestring = new MultiLineString(new LineString[] { lineString });
            extent = StacGeometryHelpers.GetBoundingBox(mlinestring);
            Assert.Equal<IPosition>(extentCheck.First().ToArray(), extent);
            var polygon = new Polygon(new LineString[] { lineString });
            extent = StacGeometryHelpers.GetBoundingBox(polygon);
            Assert.Equal<IPosition>(extentCheck.First().ToArray(), extent);
            var mpolygon = new MultiPolygon(new Polygon[] { polygon });
            extent = StacGeometryHelpers.GetBoundingBox(mpolygon);
            Assert.Equal<IPosition>(extentCheck.First().ToArray(), extent);
            var gcollection = new GeometryCollection(new IGeometryObject[] { polygon, lineString });
            // extent = StacGeometryHelpers.GetBoundingBox(gcollection);
            // Assert.Equal<IPosition>(extentCheck.First().ToArray(), extent);
        }

        [Fact]
        public void ItemClone()
        {
            var simpleJson = GetJson("Item", "ItemCloneIn");
            ValidateJson(simpleJson);
            StacItem simpleItem = StacConvert.Deserialize<StacItem>(simpleJson);
            StacItem simpleItemClone = new StacItem(simpleItem);

            var clonedJson = StacConvert.Serialize(simpleItemClone);
            ValidateJson(clonedJson);

            var expectedJson = GetJson("Item");
            JsonAssert.AreEqual(simpleJson, expectedJson);

            simpleItemClone = (StacItem)simpleItem.Clone();

            clonedJson = StacConvert.Serialize(simpleItemClone);
            ValidateJson(clonedJson);

            JsonAssert.AreEqual(simpleJson, expectedJson);
        }

        [Fact]
        public void ItemProviders()
        {
            var simpleJson = GetJson("Item", "ItemProvidersIn");
            ValidateJson(simpleJson);
            StacItem simpleItem = StacConvert.Deserialize<StacItem>(simpleJson);

            simpleItem.Providers.Add(new StacProvider("ESA", new StacProviderRole[] { StacProviderRole.licensor }));

            Assert.Contains<string, object>("providers", simpleItem.Properties);

            simpleItem.Providers.RemoveAt(0);

            Assert.DoesNotContain<string, object>("providers", simpleItem.Properties);

            var newJson = StacConvert.Serialize(simpleItem);
            ValidateJson(newJson);

        }

        [Fact]
        public void EmptyCollection()
        {
            var simpleJson = GetJson("Item", "EmptyCollectionIn");
            StacItem simpleItem = JsonConvert.DeserializeObject<StacItem>(simpleJson);
            var newJson = StacConvert.Serialize(simpleItem);
            Assert.Throws<InvalidStacDataException>(() => ValidateJson(newJson));

        }

        [Fact]
        public void ItemLinkExtensions()
        {
            var simpleJson = GetJson("Item", "ItemLinkExtensions");
            ValidateJson(simpleJson);
            StacItem simpleItem = StacConvert.Deserialize<StacItem>(simpleJson);

            Assert.Equal("GET", simpleItem.Links.First().AdditionalProperties["method"]);
        }

        [Fact]
        public void NullDateTime()
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

            StacItem item = new StacItem("notime", geometry);
            item.DateTime = null;
            string json = StacConvert.Serialize(item);
            Assert.Throws<InvalidStacDataException>(() => ValidateJson(json));

        }
    }
}
