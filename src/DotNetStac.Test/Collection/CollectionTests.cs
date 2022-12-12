using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Collection;
using Xunit;

namespace Stac.Test.Collection
{
    public class CollectionTests : TestBase
    {
        [Fact]
        public void CanDeserializeSentinel2Sample()
        {
            var json = GetJson("Collection");

            ValidateJson(json);

            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);

            var item = StacConvert.Deserialize<StacCollection>(stream);

            Assert.NotNull(item);

            Assert.NotNull(item.Summaries);

            Assert.Equal("1.0.0", item.StacVersion);

            Assert.Empty(item.StacExtensions);

            Assert.NotEmpty(item.Summaries);

            Assert.True(item.Summaries.ContainsKey("datetime"));
            Assert.True(item.Summaries.ContainsKey("platform"));
            Assert.True(item.Summaries.ContainsKey("constellation"));
            Assert.True(item.Summaries.ContainsKey("view:off_nadir"));
            Assert.True(item.Summaries.ContainsKey("eo:bands"));

            Assert.IsType<StacSummaryRangeObject<DateTime>>(item.Summaries["datetime"]);

            Assert.Equal<DateTime>(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), (item.Summaries["datetime"] as StacSummaryRangeObject<DateTime>).Min);

            Assert.Equal<long>(32601, (item.Summaries["proj:epsg"] as StacSummaryValueSet<long>).Min());
            Assert.Equal<long>(32660, (item.Summaries["proj:epsg"] as StacSummaryValueSet<long>).Max());
            Assert.Equal<long>(60, (item.Summaries["proj:epsg"] as StacSummaryValueSet<long>).Count);

            Assert.Equal(13, item.Summaries["eo:bands"].LongCount());
            Assert.Equal("B1", item.Summaries["eo:bands"][0]["name"]);
            Assert.Equal(4.439, item.Summaries["eo:bands"][0]["center_wavelength"]);

            Assert.Equal(2, item.Summaries["view:sun_elevation"].LongCount());

        }

        [Fact]
        public void CanSerializeSentinel2Sample()
        {
            StacExtent extent = new StacExtent(
                    new StacSpatialExtent(-180, -56, 180, 83),
                    new StacTemporalExtent(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), null)
            );

            StacCollection collection = new StacCollection("COPERNICUS/S2",
                                                "Sentinel-2 is a wide-swath, high-resolution, multi-spectral\nimaging mission supporting Copernicus Land Monitoring studies,\nincluding the monitoring of vegetation, soil and water cover,\nas well as observation of inland waterways and coastal areas.\n\nThe Sentinel-2 data contain 13 UINT16 spectral bands representing\nTOA reflectance scaled by 10000. See the [Sentinel-2 User Handbook](https://sentinel.esa.int/documents/247904/685211/Sentinel-2_User_Handbook)\nfor details. In addition, three QA bands are present where one\n(QA60) is a bitmask band with cloud mask information. For more\ndetails, [see the full explanation of how cloud masks are computed.](https://sentinel.esa.int/web/sentinel/technical-guides/sentinel-2-msi/level-1c/cloud-masks)\n\nEach Sentinel-2 product (zip archive) may contain multiple\ngranules. Each granule becomes a separate Earth Engine asset.\nEE asset ids for Sentinel-2 assets have the following format:\nCOPERNICUS/S2/20151128T002653_20151128T102149_T56MNN. Here the\nfirst numeric part represents the sensing date and time, the\nsecond numeric part represents the product generation date and\ntime, and the final 6-character string is a unique granule identifier\nindicating its UTM grid reference (see [MGRS](https://en.wikipedia.org/wiki/Military_Grid_Reference_System)).\n\nFor more details on Sentinel-2 radiometric resoltuon, [see this page](https://earth.esa.int/web/sentinel/user-guides/sentinel-2-msi/resolutions/radiometric).\n",
                                                extent);

            collection.Title = "Sentinel-2 MSI: MultiSpectral Instrument, Level-1C";

            collection.Links.Add(StacLink.CreateSelfLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/COPERNICUS_S2.json")));
            collection.Links.Add(StacLink.CreateParentLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
            collection.Links.Add(StacLink.CreateRootLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
            collection.Links.Add(new StacLink(new Uri("https://scihub.copernicus.eu/twiki/pub/SciHubWebPortal/TermsConditions/Sentinel_Data_Terms_and_Conditions.pdf"), "license", "Legal notice on the use of Copernicus Sentinel Data and Service Information", null));

            collection.Keywords.Add("copernicus");
            collection.Keywords.Add("esa");
            collection.Keywords.Add("eu");
            collection.Keywords.Add("msi");
            collection.Keywords.Add("radiance");
            collection.Keywords.Add("sentinel");

            collection.Providers.Add(new StacProvider("European Union/ESA/Copernicus",
                            new List<StacProviderRole>() { StacProviderRole.producer, StacProviderRole.licensor })
            {
                Uri = new Uri("https://sentinel.esa.int/web/sentinel/user-guides/sentinel-2-msi")
            });

            collection.Summaries.Add("datetime",
                new StacSummaryRangeObject<DateTime>(
                    DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(),
                    DateTime.Parse("2019-07-10T13:44:56Z").ToUniversalTime()
                )
            );

            var platforms = new StacSummaryValueSet<string>();
            platforms.Add("sentinel-2a");
            platforms.Add("sentinel-2b");

            collection.Summaries.Add("platform", platforms);

            collection.Summaries.Add("constellation",
                new StacSummaryValueSet<string>(new string[] { "sentinel-2" })
            );

            collection.Summaries.Add("instruments",
                new StacSummaryValueSet<string>(new string[] { "msi" })
            );

            collection.Summaries.Add("view:off_nadir",
                new StacSummaryRangeObject<double>(
                    0.0,
                    100
                )
            );

            collection.Summaries.Add("view:sun_elevation",
                new StacSummaryRangeObject<double>(
                    6.78,
                    89.9
                )
            );

            collection.Summaries.Add("sci:citation",
               new StacSummaryValueSet<string>(new string[] { "Copernicus Sentinel data [Year]" })
            );

            collection.Summaries.Add("gsd",
                new StacSummaryValueSet<int>(new int[] {
                    10,
                    30,
                    60
                })
            );

            collection.Summaries.Add("proj:epsg",
                new StacSummaryValueSet<int>(new int[]
                    { 32601,32602,32603,32604,32605,32606,32607,32608,32609,32610,32611,32612,32613,32614,32615,32616,32617,32618,32619,32620,32621,32622,32623,32624,32625,32626,32627,32628,32629,32630,32631,32632,32633,32634,32635,32636,32637,32638,32639,32640,32641,32642,32643,32644,32645,32646,32647,32648,32649,32650,32651,32652,32653,32654,32655,32656,32657,32658,32659,32660}
                )
            );

            collection.Summaries.Add("eo:bands",
                new StacSummaryValueSet<JObject>(new JObject[] {
                    new JObject {
                        { "name", "B1" },
                        { "common_name", "coastal" },
                        { "center_wavelength", 4.439 }
                    },
                    new JObject {
                        { "name", "B2"},
                        { "common_name", "blue"},
                        { "center_wavelength", 4.966}
                    },
                    new JObject {
                        { "name", "B3"},
                        { "common_name", "green"},
                        { "center_wavelength", 5.6}
                    },
                    new JObject {
                        { "name", "B4"},
                        { "common_name", "red"},
                        { "center_wavelength", 6.645}
                    },
                    new JObject {
                        { "name", "B5"},
                        { "center_wavelength", 7.039}
                    },
                    new JObject {
                        { "name", "B6"},
                        { "center_wavelength", 7.402}
                    },
                    new JObject {
                        { "name", "B7"},
                        { "center_wavelength", 7.825}
                    },
                    new JObject {
                        { "name", "B8"},
                        { "common_name", "nir"},
                        { "center_wavelength", 8.351}
                    },
                    new JObject {
                        { "name", "B8A"},
                        { "center_wavelength", 8.648}
                    },
                    new JObject {
                        { "name", "B9"},
                        { "center_wavelength", 9.45}
                    },
                    new JObject {
                        { "name", "B10"},
                        { "center_wavelength", 1.3735}
                    },
                    new JObject {
                        { "name", "B11"},
                        { "common_name", "swir16"},
                        { "center_wavelength", 1.6137}
                    },
                    new JObject {
                        { "name", "B12"},
                        { "common_name", "swir22"},
                        { "center_wavelength", 2.2024}
                    }
                })
            );

            var actualJson = JsonConvert.SerializeObject(collection);

            ValidateJson(actualJson);

            Console.WriteLine(actualJson);

            var expectedJson = GetJson("Collection");

            ValidateJson(expectedJson);

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public void RangeSummaryException()
        {
            var json = GetJson("Collection");
            JObject wrongSummary = JObject.Parse(json);
            Assert.Throws<ArgumentException>(() => new StacSummaryRangeObject<int>(wrongSummary));
        }

        [Fact]
        public void CollectionStacObjectLink()
        {
            var simpleJson = GetJson("Collection", "CanDeserializeSentinel2Sample");
            ValidateJson(simpleJson);
            StacCollection simpleCollection = StacConvert.Deserialize<StacCollection>(simpleJson);
            StacObjectLink stacObjectLink = (StacObjectLink)StacLink.CreateObjectLink(simpleCollection, new Uri("file:///test"));
        }

        [Fact]
        public void CollectionClone()
        {
            var simpleJson = GetJson("Collection");
            ValidateJson(simpleJson);
            StacCollection simpleCollection = StacConvert.Deserialize<StacCollection>(simpleJson);
            StacCollection simpleCollectionClone = new StacCollection(simpleCollection);

            var clonedJson = StacConvert.Serialize(simpleCollectionClone);
            ValidateJson(clonedJson);

            JsonAssert.AreEqual(simpleJson, clonedJson);

            simpleCollectionClone = (StacCollection)simpleCollection.Clone();

            clonedJson = StacConvert.Serialize(simpleCollectionClone);
            ValidateJson(clonedJson);

            JsonAssert.AreEqual(simpleJson, clonedJson);
        }

        [Fact]
        public void UpdateCollection()
        {
            var collectionJson = GetJson("Collection", "CanDeserializeSentinel2Sample");

            ValidateJson(collectionJson);

            var collection = StacConvert.Deserialize<StacCollection>(collectionJson);

            var itemJson = GetJson("Collection", "K3A_20200508102646_28267_00027320_L1G");

            ValidateJson(itemJson);

            var item = StacConvert.Deserialize<StacItem>(itemJson);

            Dictionary<Uri, StacItem> items = new Dictionary<Uri, StacItem>();
            items.Add(new Uri("http://test/test.json"), item);

            collection.Update(items);

            var actualJson = StacConvert.Serialize(collection);

            var expectedJson = GetJson("Collection");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public void CollectionSentinel1()
        {
            var simpleJson = GetJson("Collection", "sentinel-1-grd");
            ValidateJson(simpleJson);
            StacCollection simpleCollection = StacConvert.Deserialize<StacCollection>(simpleJson);
            var newJson = StacConvert.Serialize(simpleCollection);
            ValidateJson(newJson);
        }

        [Fact]
        public void CollectionTimeSeries()
        {
            var simpleJson = GetJson("Collection", "CanDeserializeTimeSeriesSample");
            ValidateJson(simpleJson);
            StacCollection simpleCollection = JsonConvert.DeserializeObject<StacCollection>(simpleJson);
            var newJson = StacConvert.Serialize(simpleCollection);
            ValidateJson(newJson);
        }

        [Fact]
        public void SerializeSimpleCollection()
        {
            var simpleItemJson = GetJson("Collection", "CanSerializeMinimalSample");
            var simpleItem = StacConvert.Deserialize<StacItem>(simpleItemJson);
            StacCollection simpleCollection = StacCollection.Create("simple-collection", "Simple Collection",
                new Dictionary<Uri, StacItem> { { new Uri("item1", UriKind.Relative), simpleItem } }, "various");
            var newJson = JsonConvert.SerializeObject(simpleCollection);
            ValidateJson(newJson);
        }


    }
}
