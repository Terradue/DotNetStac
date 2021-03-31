using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stac.Test.Catalog
{
    public class StacRepositoryCollectionTests : TestBase
    {
        private readonly string currentVersion;

        public StacRepositoryCollectionTests(){
            var packageJson = httpClient.GetStringAsync("https://raw.githubusercontent.com/radiantearth/stac-spec/master/package.json").GetAwaiter().GetResult();
            JObject package = JsonConvert.DeserializeObject<JObject>(packageJson);
            currentVersion = package.Value<string>("version");
        }

        [Fact]
        public void CanDeserializeBaseCollectionExample()
        {
            var json = httpClient.GetStringAsync("https://raw.githubusercontent.com/radiantearth/stac-spec/master/examples/collection.json").GetAwaiter().GetResult();

            ValidateJson(json);

            StacCollection collection = StacConvert.Deserialize<StacCollection>(json);

            Assert.NotNull(collection);

            Assert.Equal(currentVersion, collection.StacVersion.ToString());

            string collection2json = StacConvert.Serialize(collection);

            ValidateJson(collection2json);

            JsonAssert.AreEqual(json, collection2json);
        }

    }
}