using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            catalog = StacConvert.Deserialize<StacCatalog>(GetUseCaseJson("catalog.json"));

            Assert.NotNull(catalog);
            Assert.Equal("sentinel-stac", catalog.Id);

        }

        [Fact, TestPriority(2)]
        public void LoadRootChildren()
        {
            IEnumerable<IStacObject> children = catalog.GetChildrenLinks().Select(l => StacConvert.Deserialize<IStacObject>(File.ReadAllText(l.Uri.ToString())));

            Assert.Equal(1, children.Count());

            Assert.IsAssignableFrom<StacCollection>(children.First());
        }


    }
}
