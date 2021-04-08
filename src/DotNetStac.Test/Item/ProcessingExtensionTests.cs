using Newtonsoft.Json;
using Xunit;
using Stac.Extensions.Processing;

namespace Stac.Test.Item
{
    public class ProcessingExtensionTests : TestBase
    {
        [Fact]
        public void TestObservableDictionary()
        {
            var k3CompleteJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_software");
            var k3MissingSoftwareJson = GetJson("Item", "K3A_20200508102646_28267_00027320_L1G_missing_software");

            ValidateJson(k3CompleteJson);
            ValidateJson(k3MissingSoftwareJson);

            StacItem k3MissingSoftware = StacConvert.Deserialize<StacItem>(k3MissingSoftwareJson);

            Assert.NotNull(k3MissingSoftware.ProcessingExtension().Software);
            Assert.Equal(0, k3MissingSoftware.ProcessingExtension().Software.Count);

            k3MissingSoftware.ProcessingExtension().Software.Add("proc_IPF", "2.0.1");

            Assert.Equal(1, k3MissingSoftware.ProcessingExtension().Software.Count);

            k3MissingSoftwareJson = StacConvert.Serialize(k3MissingSoftware);

            ValidateJson(k3MissingSoftwareJson);

            JsonAssert.AreEqual(k3CompleteJson, k3MissingSoftwareJson);

        }

    }
}
