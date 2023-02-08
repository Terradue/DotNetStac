// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacLinkTests.cs

using System;
using Xunit;

namespace Stac.Test.Item
{
    public class StacLinkTests : TestBase
    {
        [Fact]
        public void CreateHelpers()
        {
            new StacLink(new Uri("file:///test"));
            StacLink.CreateDerivedFromLink(new Uri("file:///test"));
            StacLink.CreateAlternateLink(new Uri("file:///test"));
            StacLink.CreateChildLink(new Uri("file:///test"));
            var stacLink = StacLink.CreateItemLink(new Uri("file:///test"), "text/plain");
            stacLink.Title = "test";
            var cloned = new StacLink(stacLink);
            cloned = stacLink.Clone() as StacLink;
        }
    }
}
