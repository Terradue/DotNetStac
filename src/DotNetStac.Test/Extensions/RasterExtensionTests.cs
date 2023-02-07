// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: RasterExtensionTests.cs

using Xunit;

namespace Stac.Test.Extensions
{
    public class RasterExtensionTests : TestBase
    {
        [Fact]
        public void Iris()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<IStacObject>(json);

            Assert.IsType<StacItem>(item);
        }

    }
}
