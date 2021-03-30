namespace Stac.Extensions.View
{
    public class ViewStacExtension : StacPropertiesContainerExtension, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/sat/v1.0.0/schema.json";
        public static string OffNadirField => "off_nadir";
        public static string IncidenceAngleField => "incidence_angle";
        public static string AzimuthField => "azimuth";
        public static string SunAzimuthField => "sun_azimuth";
        public static string SunElevationField => "sun_elevation";

        public ViewStacExtension(IStacObject stacObject) : base(JsonSchemaUrl, "view", stacObject)
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
