using System.Collections.Generic;

namespace Stac
{
    public interface ILinksCollectionObject
    {
        ICollection<StacLink> Links { get; }
    }
}
