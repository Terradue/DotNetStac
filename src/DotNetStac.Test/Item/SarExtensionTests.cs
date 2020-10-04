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
using Stac.Extensions.Sar;
using Stac.Extensions;

namespace Stac.Test.Item
{
    public class SarExtensionTests : TestBase
    {
        [Fact]
        public void CreateSatExtensionWithNullItem()
        {
            SarStacExtension sar = null;
            Assert.Throws(typeof(ExtensionNotAssignedException),
                () => sar = new SarStacExtension(null, "IW", SarCommonFrequencyBandName.C, new string[2] { "VV", "VH" }, "GRD"));

            Assert.Null(sar);
        }

    }
}
