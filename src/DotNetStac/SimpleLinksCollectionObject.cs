using System.Collections.Generic;

namespace Stac
{
    public class SimpleLinksCollectionObject : ILinksCollectionObject
    {
        public SimpleLinksCollectionObject()
        {
            Links = new List<StacLink>();
        }

        public ICollection<StacLink> Links { get; set; }
    }
}
