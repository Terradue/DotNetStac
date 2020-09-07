using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Extensions;

namespace Stac.Extensions.Sat
{
    public class SatStacExtension : AssignableStacExtension, IStacExtension
    {
        public static string OrbitStateVectorField => "orbit_state_vectors";
        public static string AscendingNodeDateTimeField => "ascending_node_datetime";

        public static string SceneCenterCoordinatesField => "scene_center_coordinates";

        public SortedDictionary<DateTime, SatOrbitStateVector> OrbitStateVectors
        {
            get
            {
                SortedDictionary<DateTime, SatOrbitStateVector> orbitStateVectors = new SortedDictionary<DateTime, SatOrbitStateVector>();
                var osvarray = GetField(OrbitStateVectorField);
                if (osvarray == null) return null;
                orbitStateVectors = SortOrbitStateVectors(osvarray as JToken);

                return orbitStateVectors;
            }
        }

        public DateTime AscendingNodeDateTime => base.GetField<DateTime>(AscendingNodeDateTimeField);

        public void LoadOrbitStateVectors()
        {
            
        }

        public double[] SceneCenterCoordinates => base.GetField<double[]>(SceneCenterCoordinatesField);
        
        private SortedDictionary<DateTime, SatOrbitStateVector> SortOrbitStateVectors(JToken osvarray)
        {
            if (!(osvarray is JArray))
                throw new FormatException(string.Format("[{0}] field {1}: not an array", Id, OrbitStateVectorField));

            var osvlist = osvarray.ToObject<List<SatOrbitStateVector>>();

            return new SortedDictionary<DateTime, SatOrbitStateVector>(osvlist.ToDictionary(osv => osv.Time, osv => osv));
        }

        public SatStacExtension() : base("sat")
        {
        }


    }
}
