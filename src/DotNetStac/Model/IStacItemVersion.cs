using Stac.Item;

namespace Stac.Model
{
    internal interface IStacItemVersion : IStacObject
    {
        IStacItemVersion Upgrade();

    }
}