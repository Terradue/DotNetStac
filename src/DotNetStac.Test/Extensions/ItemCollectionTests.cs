using Newtonsoft.Json;
using Stac.Extensions.ItemCollections;
using Xunit;

namespace Stac.Test.ItemCollections
{
    public class ItemCollectionTests : TestBase
    {
        [Fact]
        public void CanDeserializeResto()
        {
            var json = GetJson("Extensions");

            ValidateJson(json);

            var stacObject = StacConvert.Deserialize<IStacObject>(json);

            Assert.IsAssignableFrom<ItemCollection>(stacObject);

            ItemCollection itemCollection = stacObject as ItemCollection;

            Assert.Equal("1.0.0-rc.3", itemCollection.StacVersion);

            Assert.NotEmpty(itemCollection.Features);
           
        }

    }
}