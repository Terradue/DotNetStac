﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stac.Test.Catalog
{
    public class StacRepositoryCatalogTests : TestBase
    {
        [Fact]
        public void CanDeserializeBaseCatalogExample()
        {
            var json = httpClient.GetStringAsync($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/catalog.json").GetAwaiter().GetResult();

            StacCatalog catalog = StacConvert.Deserialize<StacCatalog>(json);

            Assert.NotNull(catalog);

            Assert.Equal(Versions.StacVersionList.Current.ToString(), catalog.StacVersion.ToString());

            string catalog2json = StacConvert.Serialize(catalog);

            JsonAssert.AreEqual(json, catalog2json);
        }

    }
}
