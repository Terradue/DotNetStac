using Newtonsoft.Json.Schema;
using Stac.Exceptions;

namespace Stac.Extensions
{
    public static class StacExtensionsExtensions
    {
        public static void AddExtension(this IStacObject stacObject, JSchema jsonSchema)
        {
            if ( stacObject.StacExtensions.ContainsKey(jsonSchema.Id.ToString()))
                throw new DuplicateKeyException(jsonSchema.Id.ToString());
        }
    }
}