using Stac.Extensions;

namespace Stac
{
    public interface IStacExtensionAssignable
    {
        StacExtensions StacExtensions { get; }
    }
}