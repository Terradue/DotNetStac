using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.Raster
{
    /// <summary>
    /// Helper class to access the fields deined by the <seealso href="https://github.com/stac-extensions/raster">Raster extension</seealso>
    /// </summary>
    public class RasterStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        /// Extensions identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/raster/v1.0.0/schema.json";

        private IDictionary<string, Type> itemFields;
        private const string BandsField = "raster:bands";

        internal RasterStacExtension(StacAsset stacAsset) : base(JsonSchemaUrl, stacAsset)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(BandsField, typeof(RasterBand[]));
        }

        /// <summary>
        /// An array of available bands where each object is a Band Object.
        /// </summary>
        public RasterBand[] Bands
        {
            get { return StacPropertiesContainer.GetProperty<RasterBand[]>(BandsField); }
            set { StacPropertiesContainer.SetProperty(BandsField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;

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
