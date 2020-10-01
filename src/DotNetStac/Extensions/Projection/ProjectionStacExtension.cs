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
        public static string EpsgField => "epsg";
        public static string Wkt2Field => "wkt2";

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

        public ProjectionStacExtension() : base("proj")
        {
        }

        public void SetCoordinateSystem(ProjectedCoordinateSystem projectedCoordinateSystem)
        {
            if ( projectedCoordinateSystem.Projection.Authority != "EPSG" )
                throw new NotSupportedException("Supporting only EPSG Authority projection");

            Epsg = projectedCoordinateSystem.AuthorityCode;
            Wkt2 = projectedCoordinateSystem.WKT;
        }
    }
}
