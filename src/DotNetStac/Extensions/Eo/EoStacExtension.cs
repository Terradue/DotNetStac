﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: EoStacExtension.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.Eo
{
    /// <summary>
    /// Helper class to access the fields deined by the <seealso href="https://github.com/stac-extensions/eo">EO extension</seealso>
    /// </summary>
    public class EoStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/eo/v1.0.0/schema.json";

        private const string BandsField = "eo:bands";
        private const string CloudCoverField = "eo:cloud_cover";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private readonly IDictionary<string, Type> _itemFields;

        internal EoStacExtension(IStacPropertiesContainer stacpropertiesContainer)
            : base(JsonSchemaUrl, stacpropertiesContainer)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(BandsField, typeof(EoBandObject[]));
            this._itemFields.Add(CloudCoverField, typeof(double));
        }

        /// <summary>
        /// Gets or sets estimate of cloud cover
        /// </summary>
        public double? CloudCover
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double?>(CloudCoverField);
            }

            set
            {
                if (value == null)
                {
                    this.StacPropertiesContainer.RemoveProperty(CloudCoverField);
                }
                else
                {
                    this.StacPropertiesContainer.SetProperty(CloudCoverField, value);
                    this.DeclareStacExtension();
                }
            }
        }

        /// <summary>
        /// Gets or sets an array of available bands where each object is a Band Object.
        /// </summary>
        public EoBandObject[] Bands
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<EoBandObject[]>(BandsField);
            }

            set
            {
                if (value == null || value.Count() == 0)
                {
                    this.StacPropertiesContainer.RemoveProperty(BandsField);
                }
                else
                {
                    this.StacPropertiesContainer.SetProperty(BandsField, value);
                    this.DeclareStacExtension();
                }
            }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            summaryFunctions.Add(CloudCoverField, new SummaryFunction<double>(this, CloudCoverField, CreateRangeSummaryObject));
            return summaryFunctions;
        }
    }
}
