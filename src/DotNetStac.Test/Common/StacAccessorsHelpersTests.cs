using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Stac.Collection;
using Stac.Common;
using Xunit;

namespace Stac.Test.Item
{
    public class StacAccessorsHelpersTests : TestBase
    {
        [Fact]
        public void GetPropertyTest()
        {
            var coordinates = new[]
            {
                new List<IPosition>
                {
                    new Position(37.488035566,-122.308150179),
                    new Position(37.538869539,-122.597502109),
                    new Position(37.613537207,-122.576687533),
                    new Position(37.562818007,-122.288048600),
                    new Position(37.488035566,-122.308150179)
                }
            };

            var geometry = new Polygon(new LineString[] { new LineString(coordinates[0]) });

            var properties = new Dictionary<string, object>();

            properties.Add("collection", "CS3");

            StacItem item = new StacItem("CS3-20160503_132130_04", geometry, properties);

            DataType? dataType = null;

            item.SetProperty("test", dataType);

            Assert.Null(item.GetProperty<DataType?>("test"));

            dataType = DataType.int8;

            item.SetProperty("test", dataType);

            Assert.Equal(dataType, item.GetProperty<DataType?>("test"));

            string itemJson = StacConvert.Serialize(item);

            item = StacConvert.Deserialize<StacItem>(itemJson);

            Assert.Equal(dataType, item.GetProperty<DataType?>("test"));


            SummaryItemType summaryItemType = default(SummaryItemType);

            item.SetProperty("summary", summaryItemType);

            Assert.NotNull(item.GetProperty<SummaryItemType>("summary"));

            summaryItemType = SummaryItemType.RangeObject;

            item.SetProperty("summary", summaryItemType);

            Assert.Equal(summaryItemType, item.GetProperty<SummaryItemType>("summary"));

            itemJson = StacConvert.Serialize(item);

            item = StacConvert.Deserialize<StacItem>(itemJson);

            Assert.Equal(summaryItemType, item.GetProperty<SummaryItemType>("summary"));

        }
    }
}
