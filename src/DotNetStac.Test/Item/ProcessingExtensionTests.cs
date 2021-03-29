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

namespace Stac.Test.Item
{
    public class ProcessingExtensionTests : TestBase
    {
        [Fact]
        public void TestObservableDictionary()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_software");
            var k3MissingSoftwareJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_missing_software");

            StacItem k3MissingSoftware = JsonConvert.DeserializeObject<StacItem>(k3MissingSoftwareJson);

            ProcessingStacExtension k3MissingSoftwareProc = k3MissingSoftware.GetExtension<ProcessingStacExtension>();

            Assert.Null(k3MissingSoftwareProc);

            k3MissingSoftwareProc = new ProcessingStacExtension(k3MissingSoftware);

            Assert.NotNull(k3MissingSoftwareProc.Software);

            k3MissingSoftwareProc.Software.Add("proc_IPF", "2.0.1");

            k3MissingSoftwareJson = JsonConvert.SerializeObject(k3MissingSoftware);

            // Assert.True(ValidateJson(k3MissingSoftwareJson));

            JsonAssert.AreEqual(k3CompleteJson, k3MissingSoftwareJson);

        }

    }
}
