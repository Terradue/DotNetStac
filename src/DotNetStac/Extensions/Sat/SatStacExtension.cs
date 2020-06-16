using System;
using Stac;
using Stac.Extensions;

namespace DotNetStac.Extensions.Sat
{
    public class SatStacExtension : IStacExtension
    {
        public SatStacExtension()
        {
        }

        public string Id => "sat";

        public IStacExtension CopyForStacObject(IStacObject stacObject)
        {
            return new SatStacExtension();
        }
    }
}
