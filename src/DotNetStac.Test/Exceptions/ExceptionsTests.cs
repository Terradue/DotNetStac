using System;
using Xunit;

namespace Stac.Test.Exceptions
{
    public class ExceptionsTests : TestBase
    {
        [Fact]
        public void StacObjectLink()
        {
            var simpleJson = GetJson("Exceptions", "MinimalSample");
            ValidateJson(simpleJson);
            StacItem simpleitem = StacConvert.Deserialize<StacItem>(simpleJson);
            StacObjectLink stacObjectLink = (StacObjectLink)StacLink.CreateObjectLink(simpleitem, new Uri("file:///test"));
            Assert.Throws<InvalidOperationException>(() => stacObjectLink.Title = "test");
            Assert.Throws<InvalidOperationException>(() => stacObjectLink.ContentType = new System.Net.Mime.ContentType("text/plain"));
            Assert.Equal(simpleitem, stacObjectLink.StacObject);
        }

    }
}
