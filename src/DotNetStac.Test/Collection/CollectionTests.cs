using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Collection;
using Xunit;

namespace Stac.Test.Item
{
    public class CollectionTests : TestBase
    {
        [Fact]
        public void CanDeserializeSentinel2Sample()
        {
            var json = GetExpectedJson("Collection");

            var item = JsonConvert.DeserializeObject<StacCollection>(json);

            Assert.NotNull(item);

            Assert.NotNull(item.Summaries);

            Assert.Equal("1.0.0-beta.1", item.StacVersion);

            Assert.Empty(item.StacExtensions);

            Assert.NotEmpty(item.Summaries);

            Assert.True(item.Summaries.ContainsKey("datetime"));
            Assert.True(item.Summaries.ContainsKey("platform"));
            Assert.True(item.Summaries.ContainsKey("constellation"));
            Assert.True(item.Summaries.ContainsKey("view:off_nadir"));
            Assert.True(item.Summaries.ContainsKey("eo:bands"));

            Assert.IsType<StacSummaryStatsObject<DateTime>>(item.Summaries["datetime"]);

            Assert.Equal<DateTime>(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), (item.Summaries["datetime"] as StacSummaryStatsObject<DateTime>).Min);


        }

    }
}