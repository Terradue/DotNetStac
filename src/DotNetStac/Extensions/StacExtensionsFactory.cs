using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions
{
    public class StacExtensionsFactory : IStacExtensionsFactory
    {

        Dictionary<string, IStacExtension> stacExtensionsDictionary = new Dictionary<string, IStacExtension>();

        public StacExtensionsFactory(IStacExtension[] stacExtensions)
        {
            this.stacExtensionsDictionary = stacExtensions.ToDictionary(se => se.Id, se => se);
        }

        internal StacExtensionsFactory()
        {
            this.stacExtensionsDictionary = new IStacExtension[] {
                    new Sat.SatStacExtension()
                }.ToDictionary(se => se.Id, se => se);
        }

        public static StacExtensionsFactory Default
        {
            get
            {
                return StacExtensionsFactory.CreateDefaultFactory();
            }
        }

        public static StacExtensionsFactory CreateDefaultFactory()
        {
            return new StacExtensionsFactory();
        }

        public IStacExtension CreateStacExtension(string prefix, IStacObject stacObject)
        {
            if ( stacExtensionsDictionary.ContainsKey(prefix) )
                return stacExtensionsDictionary[prefix].CopyForStacObject(stacObject);

            return GenericStacExtension.CreateForStacObject(prefix, stacObject);
        }

    }
}