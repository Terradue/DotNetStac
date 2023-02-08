// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacRepositoryCollectionTests.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stac.Test.Catalog
{
    public class StacRepositoryCollectionTests : TestBase
    {

        [Fact]
        public void CanDeserializeBaseCollectionExample()
        {
            var json = HttpClient.GetStringAsync($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json").GetAwaiter().GetResult();

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
            var expectedJson = HttpClient.GetStringAsync($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json").GetAwaiter().GetResult();
            //TMP
            expectedJson = expectedJson.Replace("\"proj:epsg\": {\n      \"minimum\": 32659,\n      \"maximum\": 32659\n    }", "\"proj:epsg\":[32659]");

            ValidateJson(expectedJson);

            Dictionary<Uri, StacItem> items = new Dictionary<Uri, StacItem>();
            Uri simpleItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/simple-item.json");
            items.Add(simpleItemUri, StacConvert.Deserialize<StacItem>(HttpClient.GetStringAsync(simpleItemUri).GetAwaiter().GetResult()));
            items[simpleItemUri].Title = "Simple Item";
            Uri coreItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/core-item.json");
            string coreItemJson = HttpClient.GetStringAsync(coreItemUri).GetAwaiter().GetResult();
            coreItemJson = coreItemJson.Replace("cool_sat2", "cool_sat1");
            items.Add(coreItemUri, StacConvert.Deserialize<StacItem>(coreItemJson));
            Uri extendedItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/extended-item.json");
            string extendedItemJson = HttpClient.GetStringAsync(extendedItemUri).GetAwaiter().GetResult();
            extendedItemJson = extendedItemJson.Replace("cool_sensor_v1", "cool_sensor_v2");
            items.Add(extendedItemUri, StacConvert.Deserialize<StacItem>(extendedItemJson));
            StacCollection collection = StacCollection.Create("simple-collection",
                                                                "A simple collection demonstrating core catalog fields with links to a couple of items",
                                                                items,
                                                                "CC-BY-4.0",
                                                                new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json"),
                                                                null);

            collection.Title = "Simple Example Collection";
            collection.Links.Insert(0, StacLink.CreateRootLink(new Uri("./collection.json", UriKind.Relative), StacCollection.MEDIATYPE, "Simple Example Collection"));
            collection.Links.Add(StacLink.CreateSelfLink(new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json"), StacCollection.MEDIATYPE));
            collection.Providers.Add(new StacProvider("Remote Data, Inc")
            {
                Description = "Producers of awesome spatiotemporal assets",
                Uri = new Uri("http://remotedata.io")
            });
            collection.Providers[0].Roles.Add(StacProviderRole.producer);
            collection.Providers[0].Roles.Add(StacProviderRole.processor);
            var orderedext = collection.StacExtensions.OrderBy(a => a).ToArray();
            collection.StacExtensions.Clear();
            collection.StacExtensions.AddRange(orderedext);

            Assert.Equal(Versions.StacVersionList.Current.ToString(), collection.StacVersion.ToString());

            string actualJson = StacConvert.Serialize(collection);

            ValidateJson(actualJson);

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

    }
}
