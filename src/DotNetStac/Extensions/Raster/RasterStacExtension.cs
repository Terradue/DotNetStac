// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: RasterStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.Raster
{
    /// <summary>
    /// Helper class to access the fields deined by the <seealso href="https://github.com/stac-extensions/raster">Raster extension</seealso>
    /// </summary>
    public class RasterStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/raster/v1.0.0/schema.json";

        public const string BandsField = "raster:bands";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private readonly IDictionary<string, Type> _itemFields;

        internal RasterStacExtension(StacAsset stacAsset)
            : base(JsonSchemaUrl, stacAsset)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(BandsField, typeof(RasterBand[]));
        }

        /// <summary>
        /// Gets or sets an array of available bands where each object is a Band Object.
        /// </summary>
        public RasterBand[] Bands
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<RasterBand[]>(BandsField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(BandsField, value);
                this.DeclareStacExtension();
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
            return summaryFunctions;
        }
    }
}
