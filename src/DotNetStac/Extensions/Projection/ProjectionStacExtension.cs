using System;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using ProjNet.CoordinateSystems;

namespace Stac.Extensions.Projection
{
    public class ProjectionStacExtension : StacPropertiesContainerExtension, IStacExtension
    {

        public const string JsonSchemaUrl = "https://stac-extensions.github.io/projection/v1.0.0/schema.json";

        public const string EpsgField = "proj:epsg";
        public const string Wkt2Field = "proj:wkt2";
        public const string ProjJsonField = "proj:projjson";
        public const string ProjGeometryField = "proj:geometry";
        public const string ProjBboxField = "proj:bbox";
        public const string ProjCentroidField = "proj:centroid";

        private readonly Dictionary<string, Type> itemFields;

        internal ProjectionStacExtension(StacItem stacItem) : base(JsonSchemaUrl, stacItem)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(EpsgField, typeof(int));
            itemFields.Add(Wkt2Field, typeof(string));
            itemFields.Add(ProjJsonField, typeof(string));
            itemFields.Add(ProjGeometryField, typeof(IGeometryObject));
            itemFields.Add(ProjBboxField, typeof(double[]));
            itemFields.Add(ProjCentroidField, typeof(CentroidObject));
        }

        public long? Epsg
        {
            get { return StacPropertiesContainer.GetProperty<long?>(EpsgField); }
            set { StacPropertiesContainer.SetProperty(EpsgField, value); DeclareStacExtension(); }
        }

        public string Wkt2
        {
            get { return StacPropertiesContainer.GetProperty<string>(Wkt2Field); }
            set { StacPropertiesContainer.SetProperty(Wkt2Field, value); DeclareStacExtension(); }
        }

        public string ProjJson
        {
            get { return StacPropertiesContainer.GetProperty<string>(ProjJsonField); }
            set { StacPropertiesContainer.SetProperty(ProjJsonField, value); DeclareStacExtension(); }
        }

        public IGeometryObject Geometry
        {
            get { return StacPropertiesContainer.GetProperty<IGeometryObject>(Wkt2Field); }
            set { StacPropertiesContainer.SetProperty(Wkt2Field, value); DeclareStacExtension(); }
        }

        public double[] Bbox
        {
            get { return StacPropertiesContainer.GetProperty<double[]>(ProjBboxField); }
            set { StacPropertiesContainer.SetProperty(ProjBboxField, value); DeclareStacExtension(); }
        }

        public CentroidObject Centroid
        {
            get { return StacPropertiesContainer.GetProperty<CentroidObject>(ProjCentroidField); }
            set { StacPropertiesContainer.SetProperty(ProjCentroidField, value); DeclareStacExtension(); }
        }

        public override IDictionary<string, Type> ItemFields => itemFields;

        public void SetCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            if (coordinateSystem.AuthorityCode > 0)
                Epsg = coordinateSystem.AuthorityCode;
            else
                Epsg = null;
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

    public class CentroidObject
    {
        double Longitude { get; set; }

        double Latitude { get; set; }
    }

    public static class ProjectionStacExtensionExtensions
    {
        public static ProjectionStacExtension ProjectionExtension(this StacItem stacItem)
        {
            return new ProjectionStacExtension(stacItem);
        }
    }
}
