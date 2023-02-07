// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ProjExtensionTests.cs

using Stac.Extensions.Projection;
using Xunit;

namespace Stac.Test.Item
{
    public class ProjExtensionTests : TestBase
    {
        [Fact]
        public void SetCoordinateSystem()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");

            ValidateJson(k3CompleteJson);

            StacItem k3Complete = StacConvert.Deserialize<StacItem>(k3CompleteJson);

            k3Complete.ProjectionExtension().SetCoordinateSystem(ProjNet.CoordinateSystems.GeocentricCoordinateSystem.WGS84);

            string k3newProjJson = StacConvert.Serialize(k3Complete);

            var expectedJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_newproj");

            ValidateJson(expectedJson);

            JsonAssert.AreEqual(expectedJson, k3newProjJson);

        }

        [Fact]
        public void ReadCoordinateSystem()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G");

            ValidateJson(k3CompleteJson);

            StacItem k3Complete = StacConvert.Deserialize<StacItem>(k3CompleteJson);

            k3Complete.ProjectionExtension().SetCoordinateSystem(4326);

            Assert.Equal("GEOGCS[\"WGS 84\", DATUM[\"WGS_1984\", SPHEROID[\"WGS 84\", 6378137, 298.257223563, AUTHORITY[\"EPSG\", \"7030\"]], AUTHORITY[\"EPSG\", \"6326\"]], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.01745329251994328, AUTHORITY[\"EPSG\", \"9122\"]], AUTHORITY[\"EPSG\", \"4326\"]]",
                         k3Complete.ProjectionExtension().Wkt2);

            ValidateJson(StacConvert.Serialize(k3Complete));

        }

    }
}
