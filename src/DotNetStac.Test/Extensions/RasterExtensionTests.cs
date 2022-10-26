using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Extensions.Eo;
using Xunit;

namespace Stac.Test.Extensions
{
    public class RasterExtensionTests : TestBase
    {
        [Fact]
        public void Iris(){
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<IStacObject>(json);

            Assert.IsType<StacItem>(item);
        }

    }
}
