using System;

namespace Stac.Extensions
{
    internal class GenericStacExtension : IStacExtension
    {
        private string prefix;

        public GenericStacExtension(string prefix)
        {
            this.prefix = prefix;
        }

        public string Id => prefix;

        internal static IStacExtension CreateForStacObject(string prefix, IStacObject stacObject)
        {
            return new GenericStacExtension(prefix);
        }

        public IStacExtension CopyForStacObject(IStacObject stacObject)
        {
            throw new NotImplementedException();
        }
    }
}