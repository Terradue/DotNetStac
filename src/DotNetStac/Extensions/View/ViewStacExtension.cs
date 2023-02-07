// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ViewStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.View
{

    public class ViewStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/view/v1.0.0/schema.json";
        private readonly Dictionary<string, Type> _itemFields;

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

        public static string OffNadirField => "view:off_nadir";

        public static string IncidenceAngleField => "view:incidence_angle";

        public static string AzimuthField => "view:azimuth";

        public static string SunAzimuthField => "view:sun_azimuth";

        public static string SunElevationField => "view:sun_elevation";

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
