using Newtonsoft.Json;
using Stac.Extensions.Version;
using Xunit;

namespace Stac.Test.Extensions
{
    public class VersionExtensionTests : TestBase
    {
        [Fact]
        public void SetVersion()
        {
            var simpleJson = GetJson("Extensions", "MinimalSample");
            ValidateJson(simpleJson);

            StacItem simpleitem = StacConvert.Deserialize<StacItem>(simpleJson);

            simpleitem.VersionExtension().Version = "1";

            Assert.Equal("1", simpleitem.Properties["version"]);

            string actualJson = JsonConvert.SerializeObject(simpleitem);

            ValidateJson(actualJson);

            string expectedJson = GetJson("Extensions");

            JsonAssert.AreEqual(expectedJson, actualJson);
        }

    }
}
