using Newtonsoft.Json;
using Xunit;
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

            k3Complete.ProjectionExtension().SetCoordinateSystem(ProjNet.CoordinateSystems.GeocentricCoordinateSystem.WGS84);

            string k3newProjJson = JsonConvert.SerializeObject(k3Complete);

            var expectedJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_newproj");

            JsonAssert.AreEqual(expectedJson, k3newProjJson);

        }

         [Fact]
        public void ReadCoordinateSystem()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");

            StacItem k3Complete = JsonConvert.DeserializeObject<StacItem>(k3CompleteJson);

            k3Complete.ProjectionExtension().SetCoordinateSystem(4326);

            Assert.Equal("GEOGCS[\"WGS 84\", DATUM[\"WGS_1984\", SPHEROID[\"WGS 84\", 6378137, 298.257223563, AUTHORITY[\"EPSG\", \"7030\"]], AUTHORITY[\"EPSG\", \"6326\"]], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.01745329251994328, AUTHORITY[\"EPSG\", \"9122\"]], AUTHORITY[\"EPSG\", \"4326\"]]",
                         k3Complete.ProjectionExtension().Wkt2);

        }

    }
}
