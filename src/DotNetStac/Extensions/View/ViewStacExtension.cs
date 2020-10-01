using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Extensions;
using Stac.Item;

namespace Stac.Extensions.View
{
    public class ViewStacExtension : AssignableStacExtension, IStacExtension
    {
        public static string OffNadirField => "off_nadir";
        public static string IncidenceAngleField => "incidence_angle";
        public static string AzimuthField => "azimuth";
        public static string SunAzimuthField => "sun_azimuth";
        public static string SunElevationField => "sun_elevation";


        public ViewStacExtension() : base("view")
        {
        }

        public double OffNadir
        {
            get { return base.GetField<double>(OffNadirField); }
            set { base.SetField(OffNadirField, value); }
        }

        public double IncidenceAngle
        {
            get { return base.GetField<double>(IncidenceAngleField); }
            set { base.SetField(IncidenceAngleField, value); }
        }

        public double Azimuth
        {
            get { return base.GetField<double>(AzimuthField); }
            set { base.SetField(AzimuthField, value); }
        }

        public double SunAzimuth
        {
            get { return base.GetField<double>(SunAzimuthField); }
            set { base.SetField(SunAzimuthField, value); }
        }

        public double SunElevation
        {
            get { return base.GetField<double>(SunElevationField); }
            set { base.SetField(SunElevationField, value); }
        }

    }
}
