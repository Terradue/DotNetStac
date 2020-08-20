using System;
using System.Linq;
using Newtonsoft.Json;
using Stac.Catalog;
using Xunit;

namespace Stac.Test.Catalog
{
    public class CatalogTests : TestBase
    {
        [Fact]
        public void CanDeserializeMinimalSample()
        {
            var json = GetExpectedJson("Catalog");

            var catalog = JsonConvert.DeserializeObject<StacCatalog>(json);

            Assert.NotNull(catalog);

            Assert.Equal("1.0.0-beta.1", catalog.StacVersion);

            Assert.Empty(catalog.StacExtensions);

            Assert.Equal("NAIP", catalog.Id);

            Assert.Equal("Catalog of NAIP Imagery", catalog.Description);

            Assert.Contains(catalog.Links, link => link.RelationshipType == "self" && link.Uri == new Uri("https://www.fsa.usda.gov/naip/catalog.json") );
            Assert.Contains(catalog.Links, link => link.RelationshipType == "child" && link.Uri == new Uri("https://www.fsa.usda.gov/naip/30087/catalog.json") );
            Assert.Contains(catalog.Links, link => link.RelationshipType == "root" && link.Uri == new Uri("https://www.fsa.usda.gov/catalog.json") );


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

            var expectedJson = GetExpectedJson("Catalog");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

    }
}