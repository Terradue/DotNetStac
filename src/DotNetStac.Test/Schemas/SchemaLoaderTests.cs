using Stac.Extensions.Sat;
using Newtonsoft.Json;
using Xunit;
using Stac.Extensions;
using Stac.Exceptions;

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
