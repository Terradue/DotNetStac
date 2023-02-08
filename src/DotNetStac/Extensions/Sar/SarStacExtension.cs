// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SarStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.Sar
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/sar">SAR extension</seealso>
    /// </summary>
    public class SarStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private readonly IDictionary<string, Type> _itemFields;

        internal SarStacExtension(IStacPropertiesContainer stacPropertiesContainer)
            : base(JsonSchemaUrl, stacPropertiesContainer)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(InstrumentModeField, typeof(string[]));
            this._itemFields.Add(FrequencyBandField, typeof(SarCommonFrequencyBandName));
            this._itemFields.Add(CenterFrequencyField, typeof(double));
            this._itemFields.Add(PolarizationsField, typeof(string[]));
            this._itemFields.Add(ProductTypeField, typeof(string));
            this._itemFields.Add(ResolutionRangeField, typeof(double));
            this._itemFields.Add(ResolutionAzimuthField, typeof(double));
            this._itemFields.Add(PixelSpacingRangeField, typeof(double));
            this._itemFields.Add(PixelSpacingAzimuthField, typeof(double));
            this._itemFields.Add(LooksRangeField, typeof(double));
            this._itemFields.Add(LooksAzimuthField, typeof(double));
            this._itemFields.Add(LooksEquivalentNumberField, typeof(double));
            this._itemFields.Add(ObservationDirectionField, typeof(string));
        }

        /// <summary>
        /// Gets or sets the instrument mode
        /// </summary>
        public string InstrumentMode
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(InstrumentModeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(InstrumentModeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the frequency band
        /// </summary>
        public SarCommonFrequencyBandName FrequencyBand
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<SarCommonFrequencyBandName>(FrequencyBandField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(FrequencyBandField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the center frequency
        /// </summary>
        public double CenterFrequency
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(CenterFrequencyField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(CenterFrequencyField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the polarizations
        /// </summary>
        public string[] Polarizations
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string[]>(PolarizationsField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(PolarizationsField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the SAR product type
        /// </summary>
        public string ProductType
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(ProductTypeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ProductTypeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the resolution range
        /// </summary>
        public double ResolutionRange
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(ResolutionRangeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ResolutionRangeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the resolution azimuth
        /// </summary>
        public double ResolutionAzimuth
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(ResolutionAzimuthField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ResolutionAzimuthField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the pixel spacing range
        /// </summary>
        public double PixelSpacingRange
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(PixelSpacingRangeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(PixelSpacingRangeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the pixel spacing azimuth
        /// </summary>
        public double PixelSpacingAzimuth
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(PixelSpacingAzimuthField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(PixelSpacingAzimuthField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the looks range
        /// </summary>
        public double LooksRange
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(LooksRangeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(LooksRangeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the looks azimuth
        /// </summary>
        public double LooksAzimuth
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(LooksAzimuthField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(LooksAzimuthField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the looks equivalent number
        /// </summary>
        public double LooksEquivalentNumber
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double>(LooksEquivalentNumberField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(LooksEquivalentNumberField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the observation direction
        /// </summary>
        public ObservationDirection? ObservationDirection
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<ObservationDirection?>(ObservationDirectionField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ObservationDirectionField, value);
                this.DeclareStacExtension();
            }
        }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => this._itemFields;
    }
}
