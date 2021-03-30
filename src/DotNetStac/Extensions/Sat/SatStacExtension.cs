using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stac.Extensions.Sat
{
    public class SatStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/sat/v1.0.0/schema.json";
        public const string OrbitStateVectorField = "orbit_state_vectors";
        public const string AscendingNodeCrossingDateTimeField = "anx_datetime";
        public const string SceneCenterCoordinatesField = "scene_center_coordinates";
        public const string RelativeOrbitField = "relative_orbit";
        public const string AbsoluteOrbitField = "absolute_orbit";
        public const string OrbitStateField = "orbit_state";
        public const string PlatformInternationalDesignatorField = "platform_international_designator";


        public SatStacExtension(IStacObject stacObject) : base(JsonSchemaUrl, "sat", stacObject)
        {
        }

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

        public DateTime AscendingNodeCrossingDateTime
        {
            get { return base.GetField<DateTime>(AscendingNodeCrossingDateTimeField); }
            set { base.SetField(AscendingNodeCrossingDateTimeField, value); }
        }

        public string PlatformInternationalDesignator
        {
            get { return base.GetField<string>(PlatformInternationalDesignatorField); }
            set { base.SetField(PlatformInternationalDesignatorField, value); }
        }

        public void LoadOrbitStateVectors()
        {

        }

        public double[] SceneCenterCoordinates => base.GetField<double[]>(SceneCenterCoordinatesField);

        public long RelativeOrbit
        {
            get { return base.GetField<long>(RelativeOrbitField); }
            set { base.SetField(RelativeOrbitField, value); }
        }

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
                throw new FormatException(string.Format("[{0}] field {1}: not an array", FieldNamePrefix, OrbitStateVectorField));

            var osvlist = osvarray.ToObject<List<SatOrbitStateVector>>();

            return new SortedDictionary<DateTime, SatOrbitStateVector>(osvlist.ToDictionary(osv => osv.Time, osv => osv));
        }

        

    }
}
