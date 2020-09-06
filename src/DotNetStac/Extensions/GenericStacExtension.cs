using System;

namespace Stac.Extensions
{
    public class GenericStacExtension : IStacExtension
    {
        private string prefix;
        private readonly IStacObject stacObject;

        public GenericStacExtension(string prefix, IStacObject stacObject)
        {
            this.prefix = prefix;
            this.stacObject = stacObject;
        }

        public virtual string Id => prefix;

        internal static IStacExtension CreateForStacObject(string prefix, IStacObject stacObject)
        {
            return new GenericStacExtension(prefix, stacObject);
        }

        public virtual IStacExtension CopyForStacObject(IStacObject stacObject)
        {
            throw new NotImplementedException();
        }

    }
}