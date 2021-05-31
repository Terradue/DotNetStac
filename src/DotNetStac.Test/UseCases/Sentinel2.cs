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
            var json = GetUseCaseJson("catalog.json");
            ValidateJson(json);

            catalog = StacConvert.Deserialize<StacCatalog>(json);

            Assert.NotNull(catalog);
            Assert.Equal("sentinel-stac", catalog.Id);

        }

        [Fact, TestPriority(2)]
        public void LoadRootChildren()
        {
            IEnumerable<IStacObject> children = catalog.GetChildrenLinks()
                .Select(l =>
                {
                    Uri childUri = new Uri(GetUseCaseFileUri("catalog.json"), l.Uri.OriginalString);
                    var childJson = File.ReadAllText(childUri.ToString().Replace("file://", ""));
                    ValidateJson(childJson);
                    return StacConvert.Deserialize<IStacObject>(childJson);
                });

            Assert.Equal(1, children.Count());

            Assert.IsAssignableFrom<StacCollection>(children.First());
        }


    }
}
