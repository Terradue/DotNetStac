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
            this.stacExtensionsDictionary.Add("sar", typeof(Sar.SarStacExtension));
        }

        public static StacExtensionsFactory @default;

        public static StacExtensionsFactory Default
        {
            get
            {
                if (@default == null)
                    @default = StacExtensionsFactory.CreateDefaultFactory();
                return @default;
            }
        }

        public static StacExtensionsFactory CreateDefaultFactory()
        {
            return new StacExtensionsFactory();
        }

        public IStacExtension InitStacExtension(string prefix, IStacObject stacObject)
        {
            if (stacExtensionsDictionary.ContainsKey(prefix))
            {
                var ctor = stacExtensionsDictionary[prefix].GetConstructor(new Type[1] { typeof(IStacObject) });
                if (ctor == null) return null;
                return (IStacExtension)ctor.Invoke(new object[1] { stacObject });
            }

            return new GenericStacExtension(prefix, stacObject);
        }

        public StacExtensions LoadStacExtensions(IEnumerable<string> extensionPrefixes, IStacObject stacObject)
        {
            StacExtensions stacExtensions = new StacExtensions();
            if (extensionPrefixes != null)
            {
                foreach (var extensionPrefix in extensionPrefixes)
                {
                    var stacExtension = InitStacExtension(extensionPrefix, stacObject);
                    if (stacExtension != null)
                        stacExtensions.Add(stacExtension);
                }
            }
            return stacExtensions;
        }

    }
}