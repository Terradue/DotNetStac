using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Catalog;
using Stac.Item;
using Xunit;

namespace Stac.Test.Version
{
    public class VersionTests : TestBase
    {
        [Fact]
        public void CanDeserializeLandsat06()
        {
            IStacCatalog cat06 = StacFactory.LoadUriAsync(GetUri("Version/landsat06")).GetAwaiter().GetResult() as IStacCatalog;

            Assert.NotNull(cat06);
            Assert.Equal("0.6.0", cat06.StacVersion);

            StacCatalog cat = cat06.UpgradeToCurrentVersion();
            Assert.Equal("1.0.0-rc.1", cat.StacVersion);

            Assert.NotEmpty(cat.GetChildren());

            var children = cat.GetChildren();

            IStacCatalog subcat06 = children.FirstOrDefault().Value;

            Assert.NotNull(subcat06);

            Assert.Equal("0.6.0", subcat06.StacVersion);

        }
    }
}