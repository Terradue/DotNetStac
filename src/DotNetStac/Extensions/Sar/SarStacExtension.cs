using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.Sar
{
    public class SarStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/sar/v1.0.0/schema.json";

        public const string InstrumentModeField = "sar:instrument_mode";
        public const string FrequencyBandField = "sar:frequency_band";
        public const string CenterFrequencyField = "sar:center_frequency";
        public const string PolarizationsField = "sar:polarizations";
        public const string ProductTypeField = "sar:product_type";
        public const string ResolutionRangeField = "sar:resolution_range";
        public const string ResolutionAzimuthField = "sar:resolution_azimuth";
        public const string PixelSpacingRangeField = "sar:pixel_spacing_range";
        public const string PixelSpacingAzimuthField = "sar:pixel_spacing_azimuth";
        public const string LooksRangeField = "sar:looks_range";
        public const string LooksAzimuthField = "sar:looks_azimuth";
        public const string LooksEquivalentNumberField = "sar:looks_equivalent_number";
        public const string ObservationDirectionField = "sar:observation_direction";
        private readonly IDictionary<string, Type> itemFields;

        internal SarStacExtension(IStacPropertiesContainer stacPropertiesContainer) : base(JsonSchemaUrl, stacPropertiesContainer)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(InstrumentModeField, typeof(string[]));
            itemFields.Add(FrequencyBandField, typeof(SarCommonFrequencyBandName));
            itemFields.Add(CenterFrequencyField, typeof(double));
            itemFields.Add(PolarizationsField, typeof(string[]));
            itemFields.Add(ProductTypeField, typeof(string));
            itemFields.Add(ResolutionRangeField, typeof(double));
            itemFields.Add(ResolutionAzimuthField, typeof(double));
            itemFields.Add(PixelSpacingRangeField, typeof(double));
            itemFields.Add(PixelSpacingAzimuthField, typeof(double));
            itemFields.Add(LooksRangeField, typeof(double));
            itemFields.Add(LooksAzimuthField, typeof(double));
            itemFields.Add(LooksEquivalentNumberField, typeof(double));
            itemFields.Add(ObservationDirectionField, typeof(string));
        }

        public string InstrumentMode
        {
            get { return StacPropertiesContainer.GetProperty<string>(InstrumentModeField); }
            set { StacPropertiesContainer.SetProperty(InstrumentModeField, value); DeclareStacExtension(); }
        }

        public SarCommonFrequencyBandName FrequencyBand
        {
            get { return StacPropertiesContainer.GetProperty<SarCommonFrequencyBandName>(FrequencyBandField); }
            set { StacPropertiesContainer.SetProperty(FrequencyBandField, value); DeclareStacExtension(); }
        }

        public double CenterFrequency
        {
            get { return StacPropertiesContainer.GetProperty<double>(CenterFrequencyField); }
            set { StacPropertiesContainer.SetProperty(CenterFrequencyField, value); DeclareStacExtension(); }
        }

        public string[] Polarizations
        {
            get { return StacPropertiesContainer.GetProperty<string[]>(PolarizationsField); }
            set { StacPropertiesContainer.SetProperty(PolarizationsField, value); DeclareStacExtension(); }
        }

        public string ProductType
        {
            get { return StacPropertiesContainer.GetProperty<string>(ProductTypeField); }
            set { StacPropertiesContainer.SetProperty(ProductTypeField, value); DeclareStacExtension(); }
        }

        public double ResolutionRange
        {
            get { return StacPropertiesContainer.GetProperty<double>(ResolutionRangeField); }
            set { StacPropertiesContainer.SetProperty(ResolutionRangeField, value); DeclareStacExtension(); }
        }

        public double ResolutionAzimuth
        {
            get { return StacPropertiesContainer.GetProperty<double>(ResolutionAzimuthField); }
            set { StacPropertiesContainer.SetProperty(ResolutionAzimuthField, value); DeclareStacExtension(); }
        }

        public double PixelSpacingRange
        {
            get { return StacPropertiesContainer.GetProperty<double>(PixelSpacingRangeField); }
            set { StacPropertiesContainer.SetProperty(PixelSpacingRangeField, value); DeclareStacExtension(); }
        }

        public double PixelSpacingAzimuth
        {
            get { return StacPropertiesContainer.GetProperty<double>(PixelSpacingAzimuthField); }
            set { StacPropertiesContainer.SetProperty(PixelSpacingAzimuthField, value); DeclareStacExtension(); }
        }

        public double LooksRange
        {
            get { return StacPropertiesContainer.GetProperty<double>(LooksRangeField); }
            set { StacPropertiesContainer.SetProperty(LooksRangeField, value); DeclareStacExtension(); }
        }

        public double LooksAzimuth
        {
            get { return StacPropertiesContainer.GetProperty<double>(LooksAzimuthField); }
            set { StacPropertiesContainer.SetProperty(LooksAzimuthField, value); DeclareStacExtension(); }
        }

        public double LooksEquivalentNumber
        {
            get { return StacPropertiesContainer.GetProperty<double>(LooksEquivalentNumberField); }
            set { StacPropertiesContainer.SetProperty(LooksEquivalentNumberField, value); DeclareStacExtension(); }
        }

        public ObservationDirection? ObservationDirection
        {
            get { return StacPropertiesContainer.GetProperty<ObservationDirection?>(ObservationDirectionField); }
            set { StacPropertiesContainer.SetProperty(ObservationDirectionField, value); DeclareStacExtension(); }
        }

        public override IDictionary<string, Type> ItemFields => itemFields;
    }

    public static class SarStacExtensionExtensions
    {
        public static SarStacExtension SarExtension(this StacItem stacItem)
        {
            return new SarStacExtension(stacItem);
        }

        public static SarStacExtension SarExtension(this StacAsset stacAsset)
        {
            return new SarStacExtension(stacAsset);
        }

        public static StacAsset GetAsset(this StacItem stacItem, string polarization)
        {
            return stacItem.Assets.Values.FirstOrDefault(a => a.SarExtension().Polarizations.Contains(polarization));
        }

        public static void Required(this SarStacExtension sarStacExtension,
                                string instrumentMode,
                                SarCommonFrequencyBandName frequencyBandName,
                                string[] polarizations,
                                string productType)
        {
            sarStacExtension.InstrumentMode = instrumentMode;
            sarStacExtension.FrequencyBand = frequencyBandName;
            sarStacExtension.Polarizations = polarizations;
            sarStacExtension.ProductType = productType;
        }
    }


}
