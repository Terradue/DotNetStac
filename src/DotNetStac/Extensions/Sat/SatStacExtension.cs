// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SatStacExtension.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stac.Extensions.Sat
{
    /// <summary>
    /// Satellite extension
    /// </summary>
    public class SatStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Elements should be documented
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/sat/v1.0.0/schema.json";
        public const string OrbitStateVectorField = "sat:orbit_state_vectors";
        public const string AscendingNodeCrossingDateTimeField = "sat:anx_datetime";
        public const string SceneCenterCoordinatesField = "sat:scene_center_coordinates";
        public const string RelativeOrbitField = "sat:relative_orbit";
        public const string AbsoluteOrbitField = "sat:absolute_orbit";
        public const string OrbitStateField = "sat:orbit_state";
        public const string PlatformInternationalDesignatorField = "sat:platform_international_designator";
#pragma warning restore CS1591 // Elements should be documented

        private readonly Dictionary<string, Type> _itemFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="SatStacExtension"/> class.
        /// </summary>
        /// <param name="stacItem">The stac item.</param>
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

        /// <summary>
        /// Gets the orbit state vectors.
        /// </summary>
        /// <value>
        /// The orbit state vectors.
        /// </value>
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

        /// <summary>
        /// Gets or sets the ascending node crossing date time.
        /// </summary>
        /// <value>
        /// The ascending node crossing date time.
        /// </value>
        public DateTime AscendingNodeCrossingDateTime
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<DateTime>(AscendingNodeCrossingDateTimeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(AscendingNodeCrossingDateTimeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the platform international designator.
        /// </summary>
        /// <value>
        /// The platform international designator.
        /// </value>
        public string PlatformInternationalDesignator
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(PlatformInternationalDesignatorField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(PlatformInternationalDesignatorField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the scene center coordinates.
        /// </summary>
        /// <value>
        /// The scene center coordinates.
        /// </value>
        public double[] SceneCenterCoordinates
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double[]>(SceneCenterCoordinatesField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(SceneCenterCoordinatesField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        ///  Gets or sets the relative orbit.
        /// </summary>
        /// <value>
        /// The relative orbit.
        /// </value>
        public int RelativeOrbit
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<int>(RelativeOrbitField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(RelativeOrbitField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the absolute orbit.
        /// </summary>
        /// <value>
        /// The absolute orbit.
        /// </value>
        public int AbsoluteOrbit
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<int>(AbsoluteOrbitField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(AbsoluteOrbitField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the orbit state.
        /// </summary>
        /// <value>
        /// The orbit state.
        /// </value>
        public string OrbitState
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(OrbitStateField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(OrbitStateField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets the stac item.
        /// </summary>
        /// <value>
        /// The stac item.
        /// </value>
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
}
