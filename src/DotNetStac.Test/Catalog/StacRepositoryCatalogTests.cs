using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stac.Test.Catalog
{
    public class StacRepositoryCatalogTests : TestBase
    {
        private readonly string currentVersion;

        public StacRepositoryCatalogTests(){
            var packageJson = httpClient.GetStringAsync("https://raw.githubusercontent.com/radiantearth/stac-spec/master/package.json").GetAwaiter().GetResult();
            JObject package = JsonConvert.DeserializeObject<JObject>(packageJson);
            currentVersion = package.Value<string>("version");
        }

        [Fact]
        public void CanDeserializeBaseCatalogExample()
        {
            var json = httpClient.GetStringAsync("https://raw.githubusercontent.com/radiantearth/stac-spec/master/examples/catalog.json").GetAwaiter().GetResult();

            StacCatalog catalog = StacConvert.Deserialize<StacCatalog>(json);

            Assert.NotNull(catalog);

            Assert.Equal(currentVersion, catalog.StacVersion.ToString());

            string catalog2json = StacConvert.Serialize(catalog);

            JsonAssert.AreEqual(json, catalog2json);
        }

    }
}