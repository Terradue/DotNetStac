using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions
{
    public class StacExtensionsFactory : IStacExtensionsFactory
    {

        Dictionary<string, Type> stacExtensionsDictionary = new Dictionary<string, Type>();

        public StacExtensionsFactory(Dictionary<string, Type> stacExtensions)
        {
            this.stacExtensionsDictionary = stacExtensions;
        }

        internal StacExtensionsFactory()
        {
            this.stacExtensionsDictionary.Add("sat", typeof(Sat.SatStacExtension));
            this.stacExtensionsDictionary.Add("eo", typeof(Eo.EoStacExtension));
            this.stacExtensionsDictionary.Add("view", typeof(View.ViewStacExtension));
            this.stacExtensionsDictionary.Add("proj", typeof(Projection.ProjectionStacExtension));
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

        public IStacExtension InitStacExtension(string prefix)
        {
            if (stacExtensionsDictionary.ContainsKey(prefix))
            {
                var ctor = stacExtensionsDictionary[prefix].GetConstructor(new Type[] { });
                return (IStacExtension)ctor.Invoke(new object[] { });
            }

            return new GenericStacExtension(prefix);
        }

    }
}