using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Collection;
using Xunit;

namespace Stac.Test.Collection
{
    public class CollectionTests : TestBase
    {
        [Fact]
        public void CanDeserializeSentinel2Sample()
        {
            var json = GetExpectedJson("Collection");

            var item = JsonConvert.DeserializeObject<StacCollection>(json);

            Assert.NotNull(item);

            Assert.NotNull(item.Summaries);

            Assert.Equal("1.0.0-beta.1", item.StacVersion);

            Assert.Empty(item.StacExtensions);

            Assert.NotEmpty(item.Summaries);

            Assert.True(item.Summaries.ContainsKey("datetime"));
            Assert.True(item.Summaries.ContainsKey("platform"));
            Assert.True(item.Summaries.ContainsKey("constellation"));
            Assert.True(item.Summaries.ContainsKey("view:off_nadir"));
            Assert.True(item.Summaries.ContainsKey("eo:bands"));

            Assert.IsType<StacSummaryStatsObject<DateTime>>(item.Summaries["datetime"]);

            Assert.Equal<DateTime>(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), (item.Summaries["datetime"] as StacSummaryStatsObject<DateTime>).Min);

            Assert.Equal<long>(32601, (item.Summaries["proj:epsg"] as StacSummaryValueSet<long>).Min());
            Assert.Equal<long>(32660, (item.Summaries["proj:epsg"] as StacSummaryValueSet<long>).Max());

            Assert.Equal(13, item.Summaries["eo:bands"].LongCount());
            Assert.Equal("B1", item.Summaries["eo:bands"][0]["name"]);
            Assert.Equal(4.439, item.Summaries["eo:bands"][0]["center_wavelength"]);

            Assert.Equal(2, item.Summaries["view:sun_elevation"].LongCount());



        }

        [Fact]
        public void CanSerializeSentinel2Sample()
        {
            StacExtent extent = new StacExtent();
            extent.Spatial = new StacSpatialExtent( -180, -56, 180, 83);
            extent.Temporal = new StacTemporalExtent(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), null);

            StacCollection collection = new StacCollection("COPERNICUS/S2", 
                                                "Sentinel-2 is a wide-swath, high-resolution, multi-spectral\nimaging mission supporting Copernicus Land Monitoring studies,\nincluding the monitoring of vegetation, soil and water cover,\nas well as observation of inland waterways and coastal areas.\n\nThe Sentinel-2 data contain 13 UINT16 spectral bands representing\nTOA reflectance scaled by 10000. See the [Sentinel-2 User Handbook](https://sentinel.esa.int/documents/247904/685211/Sentinel-2_User_Handbook)\nfor details. In addition, three QA bands are present where one\n(QA60) is a bitmask band with cloud mask information. For more\ndetails, [see the full explanation of how cloud masks are computed.](https://sentinel.esa.int/web/sentinel/technical-guides/sentinel-2-msi/level-1c/cloud-masks)\n\nEach Sentinel-2 product (zip archive) may contain multiple\ngranules. Each granule becomes a separate Earth Engine asset.\nEE asset ids for Sentinel-2 assets have the following format:\nCOPERNICUS/S2/20151128T002653_20151128T102149_T56MNN. Here the\nfirst numeric part represents the sensing date and time, the\nsecond numeric part represents the product generation date and\ntime, and the final 6-character string is a unique granule identifier\nindicating its UTM grid reference (see [MGRS](https://en.wikipedia.org/wiki/Military_Grid_Reference_System)).\n\nFor more details on Sentinel-2 radiometric resoltuon, [see this page](https://earth.esa.int/web/sentinel/user-guides/sentinel-2-msi/resolutions/radiometric).\n",
                                                extent);
                                            
            collection.Links.Add(StacLink.CreateSelfLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/COPERNICUS_S2.json")));
            collection.Links.Add(StacLink.CreateParentLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
            collection.Links.Add(StacLink.CreateRootLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
            collection.Links.Add(new StacLink(new Uri("https://scihub.copernicus.eu/twiki/pub/SciHubWebPortal/TermsConditions/Sentinel_Data_Terms_and_Conditions.pdf"), "license", "Legal notice on the use of Copernicus Sentinel Data and Service Information", null));

            var actualJson = JsonConvert.SerializeObject(collection);

            Console.WriteLine(actualJson);

            var expectedJson = GetExpectedJson("Collection");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

    }
}