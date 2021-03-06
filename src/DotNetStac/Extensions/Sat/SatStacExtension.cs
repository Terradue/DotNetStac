﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stac.Extensions.Sat
{
    public class SatStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/sat/v1.0.0/schema.json";
        public const string OrbitStateVectorField = "sat:orbit_state_vectors";
        public const string AscendingNodeCrossingDateTimeField = "sat:anx_datetime";
        public const string SceneCenterCoordinatesField = "sat:scene_center_coordinates";
        public const string RelativeOrbitField = "sat:relative_orbit";
        public const string AbsoluteOrbitField = "sat:absolute_orbit";
        public const string OrbitStateField = "sat:orbit_state";
        public const string PlatformInternationalDesignatorField = "sat:platform_international_designator";
        private readonly Dictionary<string, Type> itemFields;

        public SatStacExtension(StacItem stacItem) : base(JsonSchemaUrl, stacItem)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(AscendingNodeCrossingDateTimeField, typeof(DateTime));
            itemFields.Add(SceneCenterCoordinatesField, typeof(double[]));
            itemFields.Add(RelativeOrbitField, typeof(int));
            itemFields.Add(AbsoluteOrbitField, typeof(int));
            itemFields.Add(OrbitStateField, typeof(string));
            itemFields.Add(PlatformInternationalDesignatorField, typeof(string));
        }

        public SortedDictionary<DateTime, SatOrbitStateVector> OrbitStateVectors
        {
            get
            {
                SortedDictionary<DateTime, SatOrbitStateVector> orbitStateVectors = new SortedDictionary<DateTime, SatOrbitStateVector>();
                var osvarray = StacPropertiesContainer.GetProperty(OrbitStateVectorField);
                if (osvarray == null) return null;
                orbitStateVectors = SortOrbitStateVectors(osvarray as JToken);

                return orbitStateVectors;
            }
        }

        public DateTime AscendingNodeCrossingDateTime
        {
            get { return StacPropertiesContainer.GetProperty<DateTime>(AscendingNodeCrossingDateTimeField); }
            set { StacPropertiesContainer.SetProperty(AscendingNodeCrossingDateTimeField, value); DeclareStacExtension(); }
        }

        public string PlatformInternationalDesignator
        {
            get { return StacPropertiesContainer.GetProperty<string>(PlatformInternationalDesignatorField); }
            set { StacPropertiesContainer.SetProperty(PlatformInternationalDesignatorField, value); DeclareStacExtension(); }
        }

        public double[] SceneCenterCoordinates
        {
            get { return StacPropertiesContainer.GetProperty<double[]>(SceneCenterCoordinatesField); }
            set { StacPropertiesContainer.SetProperty(SceneCenterCoordinatesField, value); DeclareStacExtension(); }
        }

        public int RelativeOrbit
        {
            get { return StacPropertiesContainer.GetProperty<int>(RelativeOrbitField); }
            set { StacPropertiesContainer.SetProperty(RelativeOrbitField, value); DeclareStacExtension(); }
        }

        public int AbsoluteOrbit
        {
            get { return StacPropertiesContainer.GetProperty<int>(AbsoluteOrbitField); }
            set { StacPropertiesContainer.SetProperty(AbsoluteOrbitField, value); DeclareStacExtension(); }
        }

        public string OrbitState
        {
            get { return StacPropertiesContainer.GetProperty<string>(OrbitStateField); }
            set { StacPropertiesContainer.SetProperty(OrbitStateField, value); DeclareStacExtension(); }
        }

        public StacItem StacItem => StacPropertiesContainer as StacItem;

        public override IDictionary<string, Type> ItemFields => itemFields;

        private SortedDictionary<DateTime, SatOrbitStateVector> SortOrbitStateVectors(JToken osvarray)
        {
            if (!(osvarray is JArray))
                throw new FormatException(string.Format("[sat] field {0}: not an array", OrbitStateVectorField));

            var osvlist = osvarray.ToObject<List<SatOrbitStateVector>>();

            return new SortedDictionary<DateTime, SatOrbitStateVector>(osvlist.ToDictionary(osv => osv.Time, osv => osv));
        }

    }

    public static class SatStacExtensionExtensions
    {
        public static SatStacExtension SatExtension(this StacItem stacItem)
        {
            return new SatStacExtension(stacItem);
        }
    }
}
