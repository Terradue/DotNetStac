using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac.Item;

namespace Stac.Extensions
{
    public class StacExtensions : Dictionary<string, IStacExtension>
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

        internal void InitStacObject(IStacObject stacObject)
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

      

    }
}