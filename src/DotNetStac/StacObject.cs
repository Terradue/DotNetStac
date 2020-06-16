using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Stac.Extensions;

namespace Stac
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class StacObject : IStacObject
    {
        private readonly string id;
        private string stacVersion;
        private Collection<StacLink> links;

        public StacObject(string id)
        {
            this.id = id;
        }

        public StacObject(string id, IEnumerable<StacLink> links) : this(id)
        {
            Links = new Collection<StacLink>(links.ToList());
        }

        public string Id => id;

        public string StacVersion
        {
            get
            {
                return stacVersion;
            }

            set
            {
                stacVersion = value;
            }
        }

        public abstract Collection<IStacExtension> StacExtensions { get; }

        public Collection<StacLink> Links
        {
            get
            {
                if (links == null)
                    links = new Collection<StacLink>();
                return links;
            }
            set
            {
                links = value;
            }
        }
    }
}
