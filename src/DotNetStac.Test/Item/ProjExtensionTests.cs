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

    }
}
