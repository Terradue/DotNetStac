// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SatStacExtension.cs

using System;
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
        private readonly Dictionary<string, Type> _itemFields;

        public SatStacExtension(StacItem stacItem)
            : base(JsonSchemaUrl, stacItem)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(AscendingNodeCrossingDateTimeField, typeof(DateTime));
            this._itemFields.Add(SceneCenterCoordinatesField, typeof(double[]));
            this._itemFields.Add(RelativeOrbitField, typeof(int));
            this._itemFields.Add(AbsoluteOrbitField, typeof(int));
            this._itemFields.Add(OrbitStateField, typeof(string));
            this._itemFields.Add(PlatformInternationalDesignatorField, typeof(string));
        }

        public SortedDictionary<DateTime, SatOrbitStateVector> OrbitStateVectors
        {
            get
            {
                SortedDictionary<DateTime, SatOrbitStateVector> orbitStateVectors = new SortedDictionary<DateTime, SatOrbitStateVector>();
                var osvarray = this.StacPropertiesContainer.GetProperty(OrbitStateVectorField);
                if (osvarray == null)
                {
                    return null;
                }

                orbitStateVectors = this.SortOrbitStateVectors(osvarray as JToken);

                return orbitStateVectors;
            }
        }

        public DateTime AscendingNodeCrossingDateTime
        {
            get { return this.StacPropertiesContainer.GetProperty<DateTime>(AscendingNodeCrossingDateTimeField); }
            set { this.StacPropertiesContainer.SetProperty(AscendingNodeCrossingDateTimeField, value); this.DeclareStacExtension(); }
        }

        public string PlatformInternationalDesignator
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(PlatformInternationalDesignatorField); }
            set { this.StacPropertiesContainer.SetProperty(PlatformInternationalDesignatorField, value); this.DeclareStacExtension(); }
        }

        public double[] SceneCenterCoordinates
        {
            get { return this.StacPropertiesContainer.GetProperty<double[]>(SceneCenterCoordinatesField); }
            set { this.StacPropertiesContainer.SetProperty(SceneCenterCoordinatesField, value); this.DeclareStacExtension(); }
        }

        public int RelativeOrbit
        {
            get { return this.StacPropertiesContainer.GetProperty<int>(RelativeOrbitField); }
            set { this.StacPropertiesContainer.SetProperty(RelativeOrbitField, value); this.DeclareStacExtension(); }
        }

        public int AbsoluteOrbit
        {
            get { return this.StacPropertiesContainer.GetProperty<int>(AbsoluteOrbitField); }
            set { this.StacPropertiesContainer.SetProperty(AbsoluteOrbitField, value); this.DeclareStacExtension(); }
        }

        public string OrbitState
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(OrbitStateField); }
            set { this.StacPropertiesContainer.SetProperty(OrbitStateField, value); this.DeclareStacExtension(); }
        }

        public StacItem StacItem => this.StacPropertiesContainer as StacItem;

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        private SortedDictionary<DateTime, SatOrbitStateVector> SortOrbitStateVectors(JToken osvarray)
        {
            if (!(osvarray is JArray))
            {
                throw new FormatException(string.Format("[sat] field {0}: not an array", OrbitStateVectorField));
            }

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
