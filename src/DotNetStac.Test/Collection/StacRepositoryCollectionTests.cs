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
            string coreItemJson = httpClient.GetStringAsync(coreItemUri).GetAwaiter().GetResult();
            coreItemJson = coreItemJson.Replace("cool_sat2", "cool_sat1");
            items.Add(coreItemUri, StacConvert.Deserialize<StacItem>(coreItemJson));
            Uri extendedItemUri = new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/extended-item.json");
            string extendedItemJson = httpClient.GetStringAsync(extendedItemUri).GetAwaiter().GetResult();
            extendedItemJson = extendedItemJson.Replace("cool_sensor_v1", "cool_sensor_v2");
            items.Add(extendedItemUri, StacConvert.Deserialize<StacItem>(extendedItemJson));

            StacCollection collection = StacCollection.Create("simple-collection",
                                                                "A simple collection demonstrating core catalog fields with links to a couple of items",
                                                                items,
                                                                "CC-BY-4.0",
                                                                new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/v{Versions.StacVersionList.Current}/examples/collection.json"),
                                                                null);
            // TEMPORARY overrides until #1080 is fixed
            // collection.Extent.Spatial.BoundingBoxes[0] = new double[4] { 172.911, 1.343, 172.955, 1.3691 };
            // collection.Extent.Temporal.Interval[0] = new DateTime?[2] { DateTime.Parse("2020-12-11T09:06:43.312000Z"), DateTime.Parse("2020-12-14T18:02:31.437000Z") };
            expectedJson = expectedJson.Replace(",\n      \"title\": \"Simple Item\"", "");
            // expectedJson = expectedJson.Replace("\"https://stac-extensions.github.io/eo/v1.0.0/schema.json\"",
            //                                     "\"https://stac-extensions.github.io/eo/v1.0.0/schema.json\", \"https://stac-extensions.github.io/projection/v1.0.0/schema.json\"");
            // expectedJson = expectedJson.Replace("rc.4", "rc.4", false, CultureInfo.CurrentCulture);
            // expectedJson = expectedJson.Replace("cool_sensor_v1", "cool_sensor_v1\",\"cool_sensor_v2");
            // expectedJson = expectedJson.Replace("\"cool_sat2\",\n      \"cool_sat1\"\n","\"cool_sat1\",\n      \"cool_sat2\"\n");
            // expectedJson = expectedJson.Replace("minimum\": 0,\n      \"maximum\": 15","maximum\": 3.8,\n      \"minimum\": 3.8");
            // expectedJson = expectedJson.Replace("minimum\": 6.78,\n      \"maximum\": 40","maximum\": 135.7,\n      \"minimum\": 135.7");
            // expectedJson = expectedJson.Replace("sun_elevation","sun_azimuth");
            // expectedJson = expectedJson.Replace("\"gsd\": {\n      \"minimum\": 0.512,\n      \"maximum\": 0.7\n    }",
            //                                     "\"gsd\": {\n      \"minimum\": 0.512,\n      \"maximum\": 0.66\n    },\n    \"eo:cloud_cover\": {\n      \"minimum\": 1.2,\n      \"maximum\": 1.2\n    },\n    \"proj:epsg\": {\n      \"minimum\": 32659,\n      \"maximum\": 32659\n    },\n    \"view:sun_elevation\": {\n      \"minimum\": 54.9,\n      \"maximum\": 54.9\n    }");
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