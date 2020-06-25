using System;
using System.Collections.Generic;
using System.Linq;
using Stac.Catalog;
using Stac.Collection;
using Stac.Test;
using Xunit;

namespace Stac.Test.UseCases
{
    [TestCaseOrderer("DotNetStac.Test.PriorityOrderer", "Sentinel2UseCase")]
    public class Sentinel2 : TestBase
    {
        [Fact, TestPriority(1)]
        public void LoadRootCatalog()
        {
            var catalog = StacCatalog.LoadUri(GetUseCaseFileUri("catalog.json")).Result;

            Assert.NotNull(catalog);
            Assert.IsType<StacCatalog>(catalog);
            Assert.Equal("sentinel-stac", catalog.Id);

            IDictionary<Uri, StacCatalog> children = catalog.GetChildren();

            Assert.Equal(1, children.Count);

            Assert.IsType<StacCollection>(children.First().Value);

        }


    }
}
