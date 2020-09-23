using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Extensions;
using Stac.Item;

namespace Stac.Extensions.Sat
{
    public class SatStacExtension : AssignableStacExtension, IStacExtension
    {
        public static string OrbitStateVectorField => "orbit_state_vectors";
        public static string AscendingNodeCrossingDateTimeField => "anx_date_time";
        public static string SceneCenterCoordinatesField => "scene_center_coordinates";
        public static string RelativeOrbitField => "relative_orbit";
        public static string AbsoluteOrbitField => "absolute_orbit";
        public static string OrbitStateField => "orbit_state";
        public static string PlatformInternationalDesignatorField => "platform_international_designator";


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

        public DateTime AscendingNodeCrossingDateTime => base.GetField<DateTime>(AscendingNodeCrossingDateTimeField);

        public string PlatformInternationalDesignator => base.GetField<string>(PlatformInternationalDesignatorField);

        public void LoadOrbitStateVectors()
        {

        }

        public double[] SceneCenterCoordinates => base.GetField<double[]>(SceneCenterCoordinatesField);

        public long RelativeOrbit => base.GetField<long>(RelativeOrbitField);
        public long AbsoluteOrbit
        {
            get { return base.GetField<long>(AbsoluteOrbitField); }
            set { base.SetField(AbsoluteOrbitField, value); }
        }

        public string OrbitState
        {
            get { return base.GetField<string>(OrbitStateField); }
            set { base.SetField(OrbitStateField, value); }
        }

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
