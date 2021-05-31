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
        /// Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/eo/v1.0.0/schema.json";

        private const string BandsField = "eo:bands";
        private const string CloudCoverField = "eo:cloud_cover";

        private static IDictionary<string, Type> itemFields;

        internal EoStacExtension(IStacPropertiesContainer stacpropertiesContainer) : base(JsonSchemaUrl, stacpropertiesContainer)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(BandsField, typeof(EoBandObject[]));
            itemFields.Add(CloudCoverField, typeof(double));
        }

        /// <summary>
        /// Estimate of cloud cover
        /// </summary>
        public double CloudCover
        {
            get { return StacPropertiesContainer.GetProperty<double>(CloudCoverField); }
            set { StacPropertiesContainer.SetProperty(CloudCoverField, value); DeclareStacExtension(); }
        }

        /// <summary>
        /// An array of available bands where each object is a Band Object.
        /// </summary>
        public EoBandObject[] Bands
        {
            get { return StacPropertiesContainer.GetProperty<EoBandObject[]>(BandsField); }
            set
            {
                if (value == null || value.Count() == 0)
                    StacPropertiesContainer.RemoveProperty(BandsField);
                else
                {
                    StacPropertiesContainer.SetProperty(BandsField, value);
                    DeclareStacExtension();
                }
            }
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        public override IDictionary<string, Type> ItemFields => itemFields;
    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class EoStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        public static EoStacExtension EoExtension(this StacItem stacItem)
        {
            return new EoStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a EoStacExtension class from a STAC asset
        /// </summary>
        public static EoStacExtension EoExtension(this StacAsset stacAsset)
        {
            return new EoStacExtension(stacAsset);
        }

        /// <summary>
        /// Get a STAC asset from a STAC item by its common name
        /// </summary>
        /// <param name="stacItem">Stac Item</param>
        /// <param name="commonName">common name</param>
        /// <returns></returns>
        public static StacAsset GetAsset(this StacItem stacItem, EoBandCommonName commonName)
        {
            return stacItem.Assets.Values.Where(a => a.EoExtension().Bands != null).FirstOrDefault(a => a.EoExtension().Bands.Any(b => b.CommonName == commonName));
        }

        /// <summary>
        /// Get a STAC EO Band object from a STAC item by its common name
        /// </summary>
        /// <param name="stacItem">Stac Item</param>
        /// <param name="commonName">common name</param>
        /// <returns></returns>
        public static EoBandObject GetBandObject(this StacItem stacItem, EoBandCommonName commonName)
        {
            return stacItem.Assets.Values.Where(a => a.EoExtension().Bands != null).Select(a => a.EoExtension().Bands.FirstOrDefault(b => b.CommonName == commonName)).First();
        }
    }
}
