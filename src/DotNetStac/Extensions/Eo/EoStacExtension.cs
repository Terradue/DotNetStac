using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Extensions;
using Stac.Item;

namespace Stac.Extensions.Eo
{
    public class EoStacExtension : AssignableStacExtension, IStacExtension
    {

        public const string Prefix = "eo";
        public const string BandsField = "bands";
        public const string CloudCoverField = "cloud_cover";

        public double CloudCover
        {
            get { return base.GetField<double>(CloudCoverField); }
            set { base.SetField(CloudCoverField, value); }
        }

        public EoStacExtension() : base(Prefix)
        {
        }

        public EoBandObject[] Bands
        {
            get { return base.GetField<EoBandObject[]>(BandsField); }
            set { base.SetField(BandsField, value); }
        }

        public EoBandObject[] GetAssetBandObjects(StacAsset stacAsset)
        {
            string key = Id + ":" + BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                return (EoBandObject[])stacAsset.Properties[key];
            return null;
        }

        public StacAsset GetAsset(EoBandCommonName commonName)
        {
            StacItem item = StacObject as StacItem;
            if ( item == null ) return null;
            return item.Assets.Values.FirstOrDefault(a => GetAssetBandObjects(a).Any(b => b.CommonName == commonName));
        }

        public void SetAssetBandObjects(StacAsset stacAsset, EoBandObject[] eoBandObjects)
        {
            string key = Id + ":" + BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                stacAsset.Properties.Remove(key);
            stacAsset.Properties.Add(key, eoBandObjects);
        }
    }
}
