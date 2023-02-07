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
        /// Extensions identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/raster/v1.0.0/schema.json";

        private readonly IDictionary<string, Type> itemFields;
        private const string BandsField = "raster:bands";

        internal RasterStacExtension(StacAsset stacAsset) : base(JsonSchemaUrl, stacAsset)
        {
            this.itemFields = new Dictionary<string, Type>();
            this.itemFields.Add(BandsField, typeof(RasterBand[]));
        }

        /// <summary>
        /// An array of available bands where each object is a Band Object.
        /// </summary>
        /// <value>
        /// <placeholder>An array of available bands where each object is a Band Object.</placeholder>
        /// </value>
        public RasterBand[] Bands
        {
            get { return this.StacPropertiesContainer.GetProperty<RasterBand[]>(BandsField); }
            set { this.StacPropertiesContainer.SetProperty(BandsField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        /// <value>
        /// <placeholder>Potential fields and their types</placeholder>
        /// </value>
        public override IDictionary<string, Type> ItemFields => this.itemFields;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }
    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class RasterStacExtensionExtensions
    {

        /// <summary>
        /// Initilize a EoStacExtension class from a STAC asset
        /// </summary>
        public static RasterStacExtension RasterExtension(this StacAsset stacAsset)
        {
            return new RasterStacExtension(stacAsset);
        }
    }
}
