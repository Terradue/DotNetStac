// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VersionExtensionTests.cs

using Newtonsoft.Json;
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
