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

    }
}
