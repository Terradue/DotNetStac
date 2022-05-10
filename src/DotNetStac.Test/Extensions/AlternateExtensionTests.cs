using System;
using System.IO;
using Newtonsoft.Json;
using Stac.Extensions.Alternate;
using Xunit;

namespace Stac.Test.Extensions
{
    public class AlternateExtensionTests : TestBase
    {
        [Fact]
        public async System.Threading.Tasks.Task SetAlternateAsset()
        {
            var simpleJson = GetJson("Extensions", "MinimalSample");
            ValidateJson(simpleJson);

            StacItem simpleitem = StacConvert.Deserialize<StacItem>(simpleJson);

            StacAsset stacAsset = StacAsset.CreateDataAsset(simpleitem,
                                                            new Uri("file:///srid.csv"),
                                                            new System.Net.Mime.ContentType("text/csv"),
                                                            "System reference Ids");
            stacAsset.AlternateExtension().AddAlternate("s3", new Uri("s3://bucket/key/srid.csv"));
            simpleitem.Assets.Add("srid", stacAsset);

            string actualJson = JsonConvert.SerializeObject(simpleitem);

            ValidateJson(actualJson);

            string expectedJson = GetJson("Extensions");

            JsonAssert.AreEqual(expectedJson, actualJson);

            Assert.Equal("s3://bucket/key/srid.csv", simpleitem.Assets["srid"].AlternateExtension().AlternateAssets["s3"].Uri.ToString());
        }

        [Fact]
        public async System.Threading.Tasks.Task LS9Alternates()
        {
            var simpleJson = GetJson("Extensions", "LS9Sample");
            ValidateJson(simpleJson);

            StacItem ls9item = StacConvert.Deserialize<StacItem>(simpleJson);

            Assert.Equal("s3://usgs-landsat/collection02/level-2/standard/oli-tirs/2022/088/084/LC09_L2SP_088084_20220405_20220407_02_T2/LC09_L2SP_088084_20220405_20220407_02_T2_thumb_small.jpeg", ls9item.Assets["thumbnail"].AlternateExtension().AlternateAssets["s3"].Uri.ToString());
        }

    }
}
