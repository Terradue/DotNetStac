using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac.Item;

namespace Stac.Extensions
{
    public class StacExtensions : Dictionary<string, IStacExtension>, IEnumerable<IStacExtension>
    {
        public StacExtensions(IEnumerable<IStacExtension> stacExtensions = null)
        {
            if (stacExtensions != null)
            {
                foreach (var stacExtension in stacExtensions)
                {
                    Add(stacExtension);
                }
            }
        }

        public void InitStacObject(IStacObject stacObject)
        {
            foreach (var stacExtension in Values.OfType<AssignableStacExtension>())
            {
                stacExtension.InitStacObject(stacObject);
            }
        }

        public void Add(IStacExtension extension)
        {
            Add(extension.Id, extension);
        }

        IEnumerator<IStacExtension> IEnumerable<IStacExtension>.GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }
    }
}