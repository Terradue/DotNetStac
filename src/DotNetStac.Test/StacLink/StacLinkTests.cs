using System;
using System.Collections.Generic;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stac.Test.Item
{
    public class StacLinkTests : TestBase
    {
        [Fact]
        public void CreateHelpers()
        {
            StacLink.CreateDerivedFromLink(new Uri("file:///test"));
            StacLink.CreateAlternateLink(new Uri("file:///test"));
            StacLink.CreateChildLink(new Uri("file:///test"));
            var stacLink = StacLink.CreateItemLink(new Uri("file:///test"), "text/plain");
            stacLink.Title = "test";
            var cloned = new StacLink(stacLink);
            Assert.Equal(stacLink, cloned);
            cloned = stacLink.Clone();
            Assert.Equal(stacLink, cloned);
        }
    }
}
