namespace Stac.Model
{
    internal interface IStacCollectionVersion : IStacObject
    {
        IStacCollectionVersion Upgrade();
    }
}