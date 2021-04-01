using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stac.Test.Catalog
{
    public class StacRepositoryCollectionTests : TestBase
    {

        [Fact]
        public void CanDeserializeBaseCollectionExample()
        {
            var json = httpClient.GetStringAsync($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json").GetAwaiter().GetResult();

            ValidateJson(json);

            StacCollection collection = StacConvert.Deserialize<StacCollection>(json);

            Assert.NotNull(collection);

            Assert.Equal(Versions.StacVersionList.Current.ToString(), collection.StacVersion.ToString());

            string collection2json = StacConvert.Serialize(collection);

            ValidateJson(collection2json);

            JsonAssert.AreEqual(json, collection2json);
        }

        [Fact]
        public void CanCreateBaseCollectionExample()
        {
            var expectedJson = httpClient.GetStringAsync($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json").GetAwaiter().GetResult();

            ValidateJson(expectedJson);

            Dictionary<Uri, StacItem> items = new Dictionary<Uri, StacItem>();
            Uri simpleItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/simple-item.json");
            items.Add(simpleItemUri, StacConvert.Deserialize<StacItem>(httpClient.GetStringAsync(simpleItemUri).GetAwaiter().GetResult()));
            Uri coreItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/core-item.json");
            items.Add(coreItemUri, StacConvert.Deserialize<StacItem>(httpClient.GetStringAsync(coreItemUri).GetAwaiter().GetResult()));
            Uri extendedItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/extended-item.json");
            items.Add(extendedItemUri, StacConvert.Deserialize<StacItem>(httpClient.GetStringAsync(extendedItemUri).GetAwaiter().GetResult()));

            StacCollection collection = StacCollection.Create(new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json"),
                                                                "simple-collection",
                                                                "A simple collection demonstrating core catalog fields with links to a couple of items",
                                                                items,
                                                                null,
                                                                "CC-BY-4.0");
            // TEMPORARY overrides until #1080 is fixed
            collection.Extent.Spatial.BoundingBoxes[0] = new double[4] { 172.911, 1.343, 172.955, 1.3691 };
            collection.Extent.Temporal.Interval[0] = new DateTime?[2] { DateTime.Parse("2020-12-11T09:06:43.312000Z"), DateTime.Parse("2020-12-14T18:02:31.437000Z") };
            expectedJson = expectedJson.Replace(",\n      \"title\": \"Simple Item\"", "");
            expectedJson = expectedJson.Replace("RC.1", "rc.1", false, CultureInfo.CurrentCulture);
            //
            collection.Title = "Simple Example Collection";
            collection.Links.Insert(0, StacLink.CreateRootLink(new Uri("./collection.json", UriKind.Relative), StacCollection.MEDIATYPE));
            collection.Links.Add(StacLink.CreateSelfLink(new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json"), StacCollection.MEDIATYPE));
            collection.Providers.Add(new Stac.Collection.StacProvider("Remote Data, Inc"){
                Description = "Producers of awesome spatiotemporal assets",
                Uri = new Uri("http://remotedata.io")
            });
            collection.Providers[0].Roles.Add(Stac.Collection.StacProviderRole.producer);
            collection.Providers[0].Roles.Add(Stac.Collection.StacProviderRole.processor);

            Assert.Equal(Versions.StacVersionList.Current.ToString(), collection.StacVersion.ToString());

            string actualJson = StacConvert.Serialize(collection);

            ValidateJson(actualJson);

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

    }
}