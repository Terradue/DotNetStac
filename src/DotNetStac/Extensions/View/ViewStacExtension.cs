using System;
using System.Collections.Generic;

namespace Stac.Extensions.View
{
    public class ViewStacExtension : StacPropertiesContainerExtension, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/sat/v1.0.0/schema.json";
        private readonly Dictionary<string, Type> itemFields;

        public static string OffNadirField => "view:off_nadir";
        public static string IncidenceAngleField => "view:incidence_angle";
        public static string AzimuthField => "view:azimuth";
        public static string SunAzimuthField => "view:sun_azimuth";
        public static string SunElevationField => "view:sun_elevation";

        public ViewStacExtension(IStacObject stacObject) : base(JsonSchemaUrl, stacObject)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(OffNadirField, typeof(double));
            itemFields.Add(IncidenceAngleField, typeof(double));
            itemFields.Add(AzimuthField, typeof(double));
            itemFields.Add(SunAzimuthField, typeof(double));
            itemFields.Add(SunElevationField, typeof(double));
        }

        public double OffNadir
        {
            get { return StacPropertiesContainer.GetProperty<double>(OffNadirField); }
            set { StacPropertiesContainer.SetProperty(OffNadirField, value); }
        }

        public double IncidenceAngle
        {
            get { return StacPropertiesContainer.GetProperty<double>(IncidenceAngleField); }
            set { StacPropertiesContainer.SetProperty(IncidenceAngleField, value); }
        }

        public double Azimuth
        {
            get { return StacPropertiesContainer.GetProperty<double>(AzimuthField); }
            set { StacPropertiesContainer.SetProperty(AzimuthField, value); }
        }

        public double SunAzimuth
        {
            get { return StacPropertiesContainer.GetProperty<double>(SunAzimuthField); }
            set { StacPropertiesContainer.SetProperty(SunAzimuthField, value); }
        }

        public double SunElevation
        {
            get { return StacPropertiesContainer.GetProperty<double>(SunElevationField); }
            set { StacPropertiesContainer.SetProperty(SunElevationField, value); }
        }

        public override IDictionary<string, Type> ItemFields => itemFields;
    }

    public static class ViewStacExtensionExtensions
    {
        public static ViewStacExtension ViewExtension(this StacItem stacItem)
        {
            return new ViewStacExtension(stacItem);
        }
    }
}
