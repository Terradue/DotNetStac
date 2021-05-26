using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Stac.Extensions.File;
using System.IO;

namespace Stac.Test.Extensions
{
    public class FileExtensionTests : TestBase
    {
        [Fact]
        public async System.Threading.Tasks.Task SetPropertiesFromFileInfo()
        {
            var simpleJson = GetJson("Extensions", "MinimalSample");
            ValidateJson(simpleJson);

            StacItem simpleitem = StacConvert.Deserialize<StacItem>(simpleJson);

            StacAsset stacAsset = StacAsset.CreateDataAsset(simpleitem,
                                                            new Uri("file:///srid.csv"),
                                                            new System.Net.Mime.ContentType("text/csv"),
                                                            "System reference Ids");
            await stacAsset.FileExtension().SetFileExtensionProperties(new System.IO.FileInfo("SRID.csv"));
            simpleitem.Assets.Add("srid", stacAsset);

            Assert.Equal<UInt64>(1536937, stacAsset.FileExtension().Size.Value);
            
            string actualJson = JsonConvert.SerializeObject(simpleitem);

            ValidateJson(actualJson);

            string expectedJson = GetJson("Extensions");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Fact]
        public async System.Threading.Tasks.Task SetPropertiesFromStream()
        {
            var simpleJson = GetJson("Extensions", "MinimalSample");
            ValidateJson(simpleJson);

            StacItem simpleitem = StacConvert.Deserialize<StacItem>(simpleJson);

            StacAsset stacAsset = StacAsset.CreateDataAsset(simpleitem,
                                                            new Uri("file:///srid.csv"),
                                                            new System.Net.Mime.ContentType("text/csv"),
                                                            "System reference Ids");
            await stacAsset.FileExtension().SetFileExtensionProperties(File.OpenRead("SRID.csv"));
            simpleitem.Assets.Add("srid", stacAsset);
            
            string actualJson = JsonConvert.SerializeObject(simpleitem);

            ValidateJson(actualJson);

            string expectedJson = GetJson("Extensions");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

    }
}
