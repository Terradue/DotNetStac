namespace Stac.Model
{
    internal interface IStacCatalogVersion : IStacObject
    {
        IStacCatalogVersion Upgrade();
    }
}