using Newtonsoft.Json.Schema;

namespace Stac.Extensions
{
    public class GenericStacExtension : StacPropertiesContainerExtension
    {
        public GenericStacExtension(JSchema jsonSchema, string fieldNamePrefix) : base(jsonSchema, fieldNamePrefix)
        {
        }

        // TODO helpers to dicover automatically the fields
    }
}