using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Extensions;
using Stac.Item;

namespace Stac.Extensions.Sar
{
    public class SarStacExtension : AssignableStacExtension, IStacExtension
    {
        public const string Prefix = "sar";

        public const string InstrumentModeField = "instrument_mode";
        public const string FrequencyBandField = "frequency_band";
        public const string CenterFrequencyField = "center_frequency";
        public const string PolarizationsField = "polarizations";
        public const string ProductTypeField = "product_type";
        public const string ResolutionRangeField = "resolution_range";
        public const string ResolutionAzimuthField = "resolution_azimuth";
        public const string PixelSpacingRangeField = "pixel_spacing_range";
        public const string PixelSpacingAzimuthField = "pixel_spacing_azimuth";
        public const string LooksRangeField = "looks_range";
        public const string LooksAzimuthField = "looks_azimuth";
        public const string LooksEquivalentNumberField = "looks_equivalent_number";
        public const string ObservationDirectionField = "observation_direction";


        public SarStacExtension(string instrumentMode, string frequencyBand, string[] polarizations, string productType) : base(Prefix)
        {
            InstrumentMode = instrumentMode;
            FrequencyBand = frequencyBand;
            Polarizations = polarizations;
            ProductType = productType;
        }

        public string InstrumentMode
        {
            get { return base.GetField<string>(InstrumentModeField); }
            set { base.SetField(InstrumentModeField, value); }
        }

        public string FrequencyBand
        {
            get { return base.GetField<string>(FrequencyBandField); }
            set { base.SetField(FrequencyBandField, value); }
        }

        public double CenterFrequency
        {
            get { return base.GetField<double>(CenterFrequencyField); }
            set { base.SetField(CenterFrequencyField, value); }
        }

        public string[] Polarizations
        {
            get { return base.GetField<string[]>(PolarizationsField); }
            set { base.SetField(PolarizationsField, value); }
        }

        public string ProductType
        {
            get { return base.GetField<string>(ProductTypeField); }
            set { base.SetField(ProductTypeField, value); }
        }

        public double ResolutionRange
        {
            get { return base.GetField<double>(ResolutionRangeField); }
            set { base.SetField(ResolutionRangeField, value); }
        }

        public double ResolutionAzimuth
        {
            get { return base.GetField<double>(ResolutionAzimuthField); }
            set { base.SetField(ResolutionAzimuthField, value); }
        }

        public double PixelSpacingRange
        {
            get { return base.GetField<double>(PixelSpacingRangeField); }
            set { base.SetField(PixelSpacingRangeField, value); }
        }

        public double PixelSpacingAzimuth
        {
            get { return base.GetField<double>(PixelSpacingAzimuthField); }
            set { base.SetField(PixelSpacingAzimuthField, value); }
        }

        public double LooksRange
        {
            get { return base.GetField<double>(LooksRangeField); }
            set { base.SetField(LooksRangeField, value); }
        }

        public double LooksAzimuth
        {
            get { return base.GetField<double>(LooksAzimuthField); }
            set { base.SetField(LooksAzimuthField, value); }
        }

        public double LooksEquivalentNumber
        {
            get { return base.GetField<double>(LooksEquivalentNumberField); }
            set { base.SetField(LooksEquivalentNumberField, value); }
        }

        public string ObservationDirection
        {
            get { return base.GetField<string>(ObservationDirectionField); }
            set { base.SetField(ObservationDirectionField, value); }
        }

        public static string[] GetAssetPolarization(StacAsset stacAsset)
        {
            string key = Prefix + ":" + PolarizationsField;
            if (stacAsset.Properties.ContainsKey(key))
                return (string[])stacAsset.Properties[key];
            return null;
        }

        public StacAsset GetAsset(string polarization)
        {
            StacItem item = StacObject as StacItem;
            if ( item == null ) return null;
            return item.Assets.Values.FirstOrDefault(a => GetAssetPolarization(a).Contains(polarization));
        }

        public static void SetAssetBandObjects(StacAsset stacAsset, string[] polarizations)
        {
            string key = Prefix + ":" + PolarizationsField;
            if (stacAsset.Properties.ContainsKey(key))
                stacAsset.Properties.Remove(key);
            stacAsset.Properties.Add(key, polarizations);
        }

    }
}
