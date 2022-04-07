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

    }
}
