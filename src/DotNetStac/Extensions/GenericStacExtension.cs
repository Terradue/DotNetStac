using System;
using Stac.Item;

namespace Stac.Extensions
{
    public class GenericStacExtension : AssignableStacExtension
    {
        public GenericStacExtension(string prefix, IStacObject stacObject) : base(prefix, stacObject)
        {
        }

    }
}