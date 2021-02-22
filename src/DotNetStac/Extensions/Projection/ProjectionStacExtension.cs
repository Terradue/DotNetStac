using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProjNet.CoordinateSystems;
using Stac;
using Stac.Extensions;
using Stac.Item;

namespace Stac.Extensions.Projection
{
    public class ProjectionStacExtension : AssignableStacExtension, IStacExtension
    {

        public const string Prefix = "proj";
        public const string EpsgField = "epsg";
        public const string Wkt2Field = "wkt2";

        public long Epsg
        {
            get { return base.GetField<long>(EpsgField); }
            set { base.SetField(EpsgField, value); }
        }


        public string Wkt2
        {
            get { return base.GetField<string>(Wkt2Field); }
            set { base.SetField(Wkt2Field, value); }
        }

        public ProjectionStacExtension(IStacObject stacObject) : base(Prefix, stacObject)
        {
        }

        public void SetCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            if (coordinateSystem.AuthorityCode > 0)
                Epsg = coordinateSystem.AuthorityCode;
            Wkt2 = coordinateSystem.WKT;
        }

        public void SetCoordinateSystem(int srid)
        {
            var cs = SRIDReader.GetCSbyID(srid);
            if (cs == null) return;
            Wkt2 = cs.WKT;
            Epsg = srid;
        }
    }
}
