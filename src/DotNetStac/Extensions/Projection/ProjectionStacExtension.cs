// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ProjectionStacExtension.cs

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
        public const string ProjShapeField = "proj:shape";
        public const string ProjTransformField = "proj:transform";

        private readonly Dictionary<string, Type> _itemFields;

        internal ProjectionStacExtension(StacItem stacItem)
            : base(JsonSchemaUrl, stacItem)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(EpsgField, typeof(int));
            this._itemFields.Add(Wkt2Field, typeof(string));
            this._itemFields.Add(ProjJsonField, typeof(string));
            this._itemFields.Add(ProjGeometryField, typeof(IGeometryObject));
            this._itemFields.Add(ProjBboxField, typeof(double[]));
            this._itemFields.Add(ProjCentroidField, typeof(CentroidObject));
            this._itemFields.Add(ProjShapeField, typeof(int[]));
            this._itemFields.Add(ProjTransformField, typeof(double[]));
        }

        internal ProjectionStacExtension(StacAsset stacAsset)
            : base(JsonSchemaUrl, stacAsset)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(EpsgField, typeof(int));
            this._itemFields.Add(Wkt2Field, typeof(string));
            this._itemFields.Add(ProjJsonField, typeof(string));
            this._itemFields.Add(ProjGeometryField, typeof(IGeometryObject));
            this._itemFields.Add(ProjBboxField, typeof(double[]));
            this._itemFields.Add(ProjCentroidField, typeof(CentroidObject));
            this._itemFields.Add(ProjShapeField, typeof(int[]));
            this._itemFields.Add(ProjTransformField, typeof(double[]));
        }

        public long? Epsg
        {
            get { return this.StacPropertiesContainer.GetProperty<long?>(EpsgField); }
            set { this.StacPropertiesContainer.SetProperty(EpsgField, value); this.DeclareStacExtension(); }
        }

        public string Wkt2
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(Wkt2Field); }
            set { this.StacPropertiesContainer.SetProperty(Wkt2Field, value); this.DeclareStacExtension(); }
        }

        public string ProjJson
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(ProjJsonField); }
            set { this.StacPropertiesContainer.SetProperty(ProjJsonField, value); this.DeclareStacExtension(); }
        }

        public IGeometryObject Geometry
        {
            get { return this.StacPropertiesContainer.GetProperty<IGeometryObject>(Wkt2Field); }
            set { this.StacPropertiesContainer.SetProperty(Wkt2Field, value); this.DeclareStacExtension(); }
        }

        public double[] Bbox
        {
            get { return this.StacPropertiesContainer.GetProperty<double[]>(ProjBboxField); }
            set { this.StacPropertiesContainer.SetProperty(ProjBboxField, value); this.DeclareStacExtension(); }
        }

        public CentroidObject Centroid
        {
            get { return this.StacPropertiesContainer.GetProperty<CentroidObject>(ProjCentroidField); }
            set { this.StacPropertiesContainer.SetProperty(ProjCentroidField, value); this.DeclareStacExtension(); }
        }

        public int[] Shape
        {
            get { return this.StacPropertiesContainer.GetProperty<int[]>(ProjShapeField); }
            set { this.StacPropertiesContainer.SetProperty(ProjShapeField, value); this.DeclareStacExtension(); }
        }

        public double[] Transform
        {
            get { return this.StacPropertiesContainer.GetProperty<double[]>(ProjTransformField); }
            set { this.StacPropertiesContainer.SetProperty(ProjTransformField, value); this.DeclareStacExtension(); }
        }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        public void SetCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            if (coordinateSystem.AuthorityCode > 0)
            {
                this.Epsg = coordinateSystem.AuthorityCode;
            }
            else
            {
                this.Epsg = null;
            }

            this.Wkt2 = coordinateSystem.WKT;
        }

        public void SetCoordinateSystem(int srid)
        {
            var cs = SRIDReader.GetCSbyID(srid);
            if (cs == null)
            {
                return;
            }

            this.Wkt2 = cs.WKT;
            this.Epsg = srid;
        }

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            summaryFunctions.Add(EpsgField, new SummaryFunction<int>(this, EpsgField, CreateSummaryValueSet));
            return summaryFunctions;
        }
    }

    public class CentroidObject
    {
        private double Longitude { get; set; }

        private double Latitude { get; set; }
    }

    public static class ProjectionStacExtensionExtensions
    {
        public static ProjectionStacExtension ProjectionExtension(this StacItem stacItem)
        {
            return new ProjectionStacExtension(stacItem);
        }

        public static ProjectionStacExtension ProjectionExtension(this StacAsset stacAsset)
        {
            return new ProjectionStacExtension(stacAsset);
        }
    }
}
