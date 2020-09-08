using Stac.Catalog;

namespace Stac
{
    public interface IStacCatalog: IStacObject
    {
        string Description { get; }
    }
}