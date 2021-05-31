using Newtonsoft.Json;
using Stac.Exceptions;
using Stac.Extensions;
using Stac.Extensions.Sat;
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
