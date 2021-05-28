using System;
using Newtonsoft.Json;
using Xunit;

namespace Stac.Test.Catalog
{
    public class CatalogTests : TestBase
    {
        [Fact]
        public void CanDeserializeMinimalSample()
        {
            var json = GetJson("Catalog");

            ValidateJson(json);

            var catalog = StacConvert.Deserialize<StacCatalog>(json);

            Assert.NotNull(catalog);

            Assert.Equal(Versions.StacVersionList.Current, catalog.StacVersion);

            Assert.Empty(catalog.StacExtensions);

            Assert.Equal("NAIP", catalog.Id);

            Assert.Equal("Catalog of NAIP Imagery", catalog.Description);

            Assert.Contains(catalog.Links, link => link.RelationshipType == "self" && link.Uri == new Uri("https://www.fsa.usda.gov/naip/catalog.json"));
            Assert.Contains(catalog.Links, link => link.RelationshipType == "child" && link.Uri == new Uri("https://www.fsa.usda.gov/naip/30087/catalog.json"));
            Assert.Contains(catalog.Links, link => link.RelationshipType == "root" && link.Uri == new Uri("https://www.fsa.usda.gov/catalog.json"));

            var json2 = StacConvert.Serialize(catalog);

            ValidateJson(json2);

            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public void CanSerializeMinimalSample()
        {

            StacCatalog collection = new StacCatalog("NAIP", "Catalog of NAIP Imagery");

            collection.Links.Add(StacLink.CreateSelfLink(new Uri("https://www.fsa.usda.gov/naip/catalog.json")));
            collection.Links.Add(new StacLink(new Uri("https://www.fsa.usda.gov/naip/30087/catalog.json"), "child", null, null));
            collection.Links.Add(StacLink.CreateRootLink(new Uri("https://www.fsa.usda.gov/catalog.json")));

            var actualJson = JsonConvert.SerializeObject(collection);

            Console.WriteLine(actualJson);

            var expectedJson = GetJson("Catalog");

            ValidateJson(expectedJson);
            ValidateJson(actualJson);

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public void CatalogStacObjectLink()
        {
            var simpleJson = GetJson("Catalog", "CanDeserializeMinimalSample");
            ValidateJson(simpleJson);
            StacCatalog simpleCollection = StacConvert.Deserialize<StacCatalog>(simpleJson);
            StacObjectLink stacObjectLink = (StacObjectLink)StacLink.CreateObjectLink(simpleCollection, new Uri("file:///test"));
        }

    }
}
