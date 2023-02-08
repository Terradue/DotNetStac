// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ViewStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.View
{
    /// <summary>
    /// View extension
    /// </summary>
    public class ViewStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/view/v1.0.0/schema.json";

        public const string OffNadirField = "view:off_nadir";

        public const string IncidenceAngleField = "view:incidence_angle";

        public const string AzimuthField = "view:azimuth";

        public const string SunAzimuthField = "view:sun_azimuth";

        public const string SunElevationField = "view:sun_elevation";

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private readonly Dictionary<string, Type> _itemFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStacExtension"/> class.
        /// </summary>
        /// <param name="stacObject">The stac object.</param>
        public ViewStacExtension(IStacObject stacObject)
                    : base(JsonSchemaUrl, stacObject)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(OffNadirField, typeof(double));
            this._itemFields.Add(IncidenceAngleField, typeof(double));
            this._itemFields.Add(AzimuthField, typeof(double));
            this._itemFields.Add(SunAzimuthField, typeof(double));
            this._itemFields.Add(SunElevationField, typeof(double));
        }

        /// <summary>
        /// Gets or sets the off nadir angle.
        /// The angle between the local surface normal and the satellite's nadir direction.
        /// </summary>
        public double OffNadir
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(OffNadirField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(OffNadirField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the incidence angle.
        /// The angle between the local surface normal and the line of sight to the satellite.
        /// </summary>
        public double IncidenceAngle
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(IncidenceAngleField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(IncidenceAngleField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the azimuth angle.
        /// The angle angle between a satellite and the North which is measured clockwise
        /// </summary>
        public double Azimuth
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(AzimuthField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(AzimuthField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the sun azimuth angle.
        /// The angle between the sun and the North which is measured clockwise
        /// </summary>
        public double SunAzimuth
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(SunAzimuthField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(SunAzimuthField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the sun elevation angle.
        /// The angle between the sun and the local surface normal
        /// </summary>
        public double SunElevation
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(SunElevationField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(SunElevationField, value);
                this.DeclareStacExtension();
            }
        }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => this._itemFields;
    }
}
