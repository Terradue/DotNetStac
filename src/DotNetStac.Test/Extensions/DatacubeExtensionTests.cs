using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Extensions.Datacube;
using Stac.Extensions.Eo;
using Xunit;

namespace Stac.Test.Extensions
{
    public class DatacubeExtensionTests : TestBase
    {
        [Fact]
        public void Datacube123()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.IsType<StacItem>(item);
            Assert.Equal(6, item.DatacubeStacExtension().Dimensions.Count);
            Assert.Equal(2, item.DatacubeStacExtension().Variables.Count);
        }

        [Fact]
        public void Sample1()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.IsType<StacItem>(item);
            Assert.Equal(3, item.DatacubeStacExtension().Dimensions.Count);
            Assert.Equal(7, item.DatacubeStacExtension().Variables.Count);
        }

        [Fact]
        public void CanSerializeDatacube123()
        {
            var originalJson = GetJson("Extensions", "Datacube123");

            var coordinates = new[]
            {
                new List<IPosition>
                {
                    new Position(37.488035566,-122.308150179),
                    new Position(37.538869539,-122.597502109),
                    new Position(37.613537207,-122.576687533),
                    new Position(37.562818007,-122.2880486),
                    new Position(37.488035566,-122.308150179)
                }
            };
            var geometry = new Polygon(new LineString[] { new LineString(coordinates[0]) });
            var properties = new Dictionary<string, object>();
            StacItem item = new StacItem("datacube-123", geometry, properties);

            item.StacExtensions.Add("https://stac-extensions.github.io/datacube/v2.1.0/schema.json");
            item.SetProperty("title", "Multi-dimensional data cube 123 in a STAC Item.");
            item.SetProperty("datetime", "2016-05-03T13:21:30.040Z");

            var xdim = new DatacubeDimensionSpatialHorizontal();
            xdim.Axis = DatacubeAxis.x;
            xdim.Type = "spatial";
            xdim.Extent = new double[] { -122.59750209, -122.2880486 };
            xdim.ReferenceSystem = 4326;
            item.DatacubeStacExtension().Dimensions.Add("x", xdim);

            var ydim = new DatacubeDimensionSpatialHorizontal();
            ydim.Axis = DatacubeAxis.y;
            ydim.Type = "spatial";
            ydim.Extent = new double[] { 37.48803556, 37.613537207 };
            ydim.ReferenceSystem = 4326;
            item.DatacubeStacExtension().Dimensions.Add("y", ydim);

            var pressure_levelsDim = new DatacubeDimensionSpatialVertical();
            pressure_levelsDim.Axis = DatacubeAxis.z;
            pressure_levelsDim.Type = "spatial";
            pressure_levelsDim.Extent = new double[] { 0, 1000 };
            pressure_levelsDim.Step = 100.0;
            pressure_levelsDim.Unit = "Pa";
            item.DatacubeStacExtension().Dimensions.Add("pressure_levels", pressure_levelsDim);

            var metered_levelsDim = new DatacubeDimensionSpatialVertical();
            metered_levelsDim.Axis = DatacubeAxis.z;
            metered_levelsDim.Type = "spatial";
            metered_levelsDim.Values = new double[] { 0, 10, 25, 50, 100, 1000 };
            metered_levelsDim.Unit = "m";
            item.DatacubeStacExtension().Dimensions.Add("metered_levels", metered_levelsDim);

            var timeDim = new DatacubeDimensionTemporal();
            timeDim.Type = "temporal";
            timeDim.Values = new string[] { "2016-05-03T13:21:30.040Z" };
            item.DatacubeStacExtension().Dimensions.Add("time", timeDim);

            var spectralDim = new DatacubeDimensionAdditional();
            spectralDim.Type = "bands";
            spectralDim.Values = new string[] { "red", "green", "blue" };
            item.DatacubeStacExtension().Dimensions.Add("spectral", spectralDim);

            var tempVar = new DatacubeVariable();
            tempVar.Dimensions = new string[] { "time", "y", "x", "pressure_levels" };
            tempVar.Type = DatacubeVariableType.data;
            item.DatacubeStacExtension().Variables.Add("temp", tempVar);

            var colorVar = new DatacubeVariable();
            colorVar.Dimensions = new string[] { };
            colorVar.Type = DatacubeVariableType.auxiliary;
            colorVar.Values = new string[] { "red", "green", "blue" };
            item.DatacubeStacExtension().Variables.Add("color", colorVar);

            var dataAsset = new StacAsset(item, new Uri("http://cool-sat.com/catalog/datacube-123/data.nc"), null, "netCDF Data cube", new ContentType("application/netcdf"));
            item.Assets.Add("data", dataAsset);

            var thumbnailAsset = new StacAsset(item, new Uri("http://cool-sat.com/catalog/datacube-123/thumbnail.png"), null, "Thumbnail", new ContentType("image/png"));
            item.Assets.Add("thumbnail", thumbnailAsset);

            var link = new StacLink(new Uri("http://cool-sat.com/catalog/datacube-123/item.json"), "self", null, null);
            item.Links.Add(link);

            var json2 = StacConvert.Serialize(item);

            Console.WriteLine("Generated JSON : ");
            Console.WriteLine(json2);

            // ValidateJson(json2);

            JsonAssert.AreEqual(originalJson, json2);
        }

    }
}
