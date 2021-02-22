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
using Stac.Extensions.Processing;
using Stac.Extensions.Projection;

namespace Stac.Test.Item
{
    public class ProjExtensionTests : TestBase
    {
        [Fact]
        public void SetCoordinateSystem()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");

            StacItem k3Complete = JsonConvert.DeserializeObject<StacItem>(k3CompleteJson);

            ProjectionStacExtension proj = k3Complete.GetExtension<ProjectionStacExtension>();

            Assert.NotNull(proj);

            proj.SetCoordinateSystem(ProjNet.CoordinateSystems.GeocentricCoordinateSystem.WGS84);

            string k3newProjJson = JsonConvert.SerializeObject(k3Complete);

            var expectedJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_newproj");

            JsonAssert.AreEqual(expectedJson, k3newProjJson);

        }

         [Fact]
        public void ReadCoordinateSystem()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");

            StacItem k3Complete = JsonConvert.DeserializeObject<StacItem>(k3CompleteJson);

            ProjectionStacExtension proj = k3Complete.GetExtension<ProjectionStacExtension>();

            Assert.NotNull(proj);

            proj.SetCoordinateSystem(4326);

            Assert.Equal("GEOGCS[\"WGS 84\", DATUM[\"WGS_1984\", SPHEROID[\"WGS 84\", 6378137, 298.257223563, AUTHORITY[\"EPSG\", \"7030\"]], AUTHORITY[\"EPSG\", \"6326\"]], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.01745329251994328, AUTHORITY[\"EPSG\", \"9122\"]], AUTHORITY[\"EPSG\", \"4326\"]]", proj.Wkt2);

        }

    }
}
