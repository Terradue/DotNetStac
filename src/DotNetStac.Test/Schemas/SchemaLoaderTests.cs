// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SchemaLoaderTests.cs

using Stac.Exceptions;
using Stac.Extensions;
using Xunit;

namespace Stac.Test.Item
{
    public class SchemaLoaderTests : TestBase
    {
        [Fact]
        public void ThrowInvalidExtensionShortcut()
        {
            Assert.Throws<InvalidStacSchemaException>(() => SchemaBasedStacExtension.Create("item", null, null));

        }

    }
}
