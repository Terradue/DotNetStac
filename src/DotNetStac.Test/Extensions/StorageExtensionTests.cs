// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StorageExtensionTests.cs

using System;
using Newtonsoft.Json;
using Stac.Extensions.Alternate;
using Xunit;

namespace Stac.Test.Extensions
{
    public class StorageExtensionTests : TestBase
    {
        [Fact]
        public System.Threading.Tasks.Task SetAlternateStorageAsset()
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
            stacAsset.AlternateExtension().AlternateAssets["s3"].StorageExtension().Platform = "scw";
            stacAsset.AlternateExtension().AlternateAssets["s3"].StorageExtension().Region = "fr-par";
            stacAsset.AlternateExtension().AlternateAssets["s3"].StorageExtension().RequesterPays = false;
            stacAsset.AlternateExtension().AlternateAssets["s3"].StorageExtension().Tier = "C14";

            string actualJson = JsonConvert.SerializeObject(simpleitem);

            ValidateJson(actualJson);

            string expectedJson = GetJson("Extensions");

            JsonAssert.AreEqual(expectedJson, actualJson);

            Assert.Equal("s3://bucket/key/srid.csv", simpleitem.Assets["srid"].AlternateExtension().AlternateAssets["s3"].Uri.ToString());
            Assert.Equal("fr-par", simpleitem.Assets["srid"].AlternateExtension().AlternateAssets["s3"].StorageExtension().Region);
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
