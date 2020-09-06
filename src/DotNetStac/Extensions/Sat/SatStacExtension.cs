using System;
using System.Collections.Generic;
using Stac;
using Stac.Extensions;

namespace Stac.Extensions.Sat
{
    public class SatStacExtension : GenericStacExtension, IStacExtension
    {
        public static string OrbitStateVectorField => "orbit_state_vectors";

        // TODO remove
        public SatStacExtension() : base("sat", null)
        {
        }

        public SatStacExtension(IStacObject stacObject) : base("sat", stacObject)
        {
        }

        public override IStacExtension CopyForStacObject(IStacObject stacObject)
        {
            return new SatStacExtension(stacObject);
        }

        public SortedDictionary<DateTime, SatOrbitSateVector> OrbitStateVectors
        {
            get
            {
                return null;
            }
        }

    }
}
