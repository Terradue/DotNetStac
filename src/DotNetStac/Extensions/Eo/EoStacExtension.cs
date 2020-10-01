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
        public static string BandsField => "bands";
        public static string CloudCoverField => "cloud_cover";

        public EoBandObject[] Bands => base.GetField<EoBandObject[]>(BandsField);

        public double CloudCover
        {
            get { return base.GetField<double>(CloudCoverField); }
            set { base.SetField(CloudCoverField, value); }
        }

        public EoStacExtension() : base("eo")
        {
        }

        public void AddBandObject(StacAsset stacAsset, EoBandObject[] eoBandObject)
        {
            string key = Id + ":" + BandsField;
            stacAsset.Properties.Remove(key);
            stacAsset.Properties.Add(key, eoBandObject);
        }

        public EoBandObject[] GetBandObject(StacAsset stacAsset)
        {
            string key = Id + ":" + BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                return (EoBandObject[])stacAsset.Properties[key];
            return null;
        }
    }
}
