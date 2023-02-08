// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: Sentinel2.cs

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
        private static StacCatalog Catalog;

        [Fact, TestPriority(1)]
        public void LoadRootCatalog()
        {
            var json = GetUseCaseJson("catalog.json");
            ValidateJson(json);

            Catalog = StacConvert.Deserialize<StacCatalog>(json);

            Assert.NotNull(Catalog);
            Assert.Equal("sentinel-stac", Catalog.Id);

        }

        [Fact, TestPriority(2)]
        public void LoadRootChildren()
        {
            IEnumerable<IStacObject> children = Catalog.GetChildrenLinks()
                .Select(l =>
                {
                    Uri childUri = new Uri(GetUseCaseFileUri("catalog.json"), l.Uri.OriginalString);
                    var childJson = File.ReadAllText(childUri.ToString().Replace("file://", ""));
                    ValidateJson(childJson);
                    return StacConvert.Deserialize<IStacObject>(childJson);
                });

            Assert.Single(children);

            Assert.IsAssignableFrom<StacCollection>(children.First());
        }


    }
}
