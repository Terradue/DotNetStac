using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.Eo
{
    public class EoStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        private static IDictionary<string, Type> itemFields;

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/eo/v1.0.0/schema.json";
        public const string BandsField = "eo:bands";
        public const string CloudCoverField = "eo:cloud_cover";

        public EoStacExtension(IStacPropertiesContainer stacpropertiesContainer) : base(JsonSchemaUrl, stacpropertiesContainer)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(BandsField, typeof(EoBandObject[]) );
            itemFields.Add(CloudCoverField, typeof(double) );
        }

        public double CloudCover
        {
            get { return StacPropertiesContainer.GetProperty<double>(CloudCoverField); }
            set { StacPropertiesContainer.SetProperty(CloudCoverField, value); }
        }

        public EoBandObject[] Bands
        {
            get { return StacPropertiesContainer.GetProperty<EoBandObject[]>(BandsField); }
            set { StacPropertiesContainer.SetProperty(BandsField, value); }
        }

        public override IDictionary<string, Type> ItemFields => itemFields;
    }

    public static class EoStacExtensionExtensions
    {
        public static EoStacExtension EoExtension(this StacItem stacItem)
        {
            return new EoStacExtension(stacItem);
        }

        public static EoStacExtension EoExtension(this StacAsset stacAsset)
        {
            return new EoStacExtension(stacAsset);
        }

        public static StacAsset GetAsset(this StacItem stacItem, EoBandCommonName commonName)
        {
            return stacItem.Assets.Values.FirstOrDefault(a => a.EoExtension().Bands.Any(b => b.CommonName == commonName));
        }
    }
}
