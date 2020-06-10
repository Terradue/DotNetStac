using System;
using Stac.Extensions;

namespace DotNetStac.Extensions.Sat
{
    public class SatStacExtension : IStacExtension
    {
        public SatStacExtension()
        {
        }

        public string Id => "sat";
    }
}
