using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stac.Item;

namespace Stac.Extensions
{
    public class StacExtensions : IEnumerable<IStacExtension>, ICollection<IStacExtension>
    {
        private IStacObject stacObject;
        private Collection<IStacExtension> stacExtensions = new Collection<IStacExtension>();

        public int Count => stacExtensions.Count;

        public bool IsReadOnly => false;

        public StacExtensions(IEnumerable<IStacExtension> stacExtensions = null)
        {
            if (stacExtensions != null)
                this.stacExtensions = new Collection<IStacExtension>(stacExtensions.ToList());
        }

        internal void InitStacObject(IStacObject stacObject)
        {
            this.stacObject = stacObject;
            foreach (var stacExtension in stacExtensions.OfType<AssignableStacExtension>())
            {
                stacExtension.InitStacObject(stacObject);
            }
        }

        public void Add(IStacExtension item)
        {
            stacExtensions.Add(item);
            if (item is AssignableStacExtension)
                (item as AssignableStacExtension).InitStacObject(stacObject);
        }

        public IEnumerator<IStacExtension> GetEnumerator()
        {
            return stacExtensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            stacExtensions.Clear();
        }

        public bool Contains(IStacExtension stacExtension)
        {
            return stacExtensions.Contains(stacExtension);
        }

        public void CopyTo(IStacExtension[] array, int arrayIndex)
        {
            stacExtensions.CopyTo(array, arrayIndex);
        }

        public bool Remove(IStacExtension stacExtension)
        {
            return stacExtensions.Remove(stacExtension);
        }
    }
}