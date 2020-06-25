using System;
using System.Collections.Generic;
using System.Linq;
using Stac.Catalog;
using Stac.Collection;
using Stac.Test;
using Xunit;

namespace Stac.Test.UseCases
{
    [TestCaseOrderer("Stac.Test.PriorityOrderer", "DotNetStac.Test")]
    public class Sentinel2 : TestBase
    {
        private static StacCatalog catalog;

        [Fact, TestPriority(1)]
        public void LoadRootCatalog()
        {
            catalog = StacCatalog.LoadUri(GetUseCaseFileUri("catalog.json")).Result;

            Assert.NotNull(catalog);
            Assert.IsType<StacCatalog>(catalog);
            Assert.Equal("sentinel-stac", catalog.Id);

        }

        [Fact, TestPriority(2)]
        public void LoadRootChildren()
        {
            IDictionary<Uri, StacCatalog> children = catalog.GetChildren();

            Assert.Equal(1, children.Count);

            Assert.IsType<StacCollection>(children.First().Value);
        }


    }
}
