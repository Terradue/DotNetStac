// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ProjectionStacExtension.cs

using System;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using ProjNet.CoordinateSystems;

namespace Stac.Extensions.Projection
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/projection">Projection extension</seealso>
    /// </summary>
    public class ProjectionStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/projection/v1.0.0/schema.json";

        public const string EpsgField = "proj:epsg";
        public const string Wkt2Field = "proj:wkt2";
        public const string ProjJsonField = "proj:projjson";
        public const string ProjGeometryField = "proj:geometry";
        public const string ProjBboxField = "proj:bbox";
        public const string ProjCentroidField = "proj:centroid";
        public const string ProjShapeField = "proj:shape";
        public const string ProjTransformField = "proj:transform";

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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

        /// <summary>
        /// Gets or sets the EPSG code.
        /// </summary>
        public long? Epsg
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<long?>(EpsgField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(EpsgField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the WKT2 string.
        /// </summary>
        public string Wkt2
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(Wkt2Field);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(Wkt2Field, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the PROJJSON string.
        /// </summary>
        public string ProjJson
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(ProjJsonField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ProjJsonField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the specific geometry.
        /// </summary>
        public IGeometryObject Geometry
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<IGeometryObject>(Wkt2Field);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(Wkt2Field, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the bounding box.
        /// </summary>
        public double[] Bbox
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double[]>(ProjBboxField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ProjBboxField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the centroid.
        /// </summary>
        public CentroidObject Centroid
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<CentroidObject>(ProjCentroidField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ProjCentroidField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        public int[] Shape
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<int[]>(ProjShapeField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ProjShapeField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the transformation matrix.
        /// </summary>
        public double[] Transform
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<double[]>(ProjTransformField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(ProjTransformField, value);
                this.DeclareStacExtension();
            }
        }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        /// <summary>
        ///  Sets the coordinate system by the given coordinate system.
        /// </summary>
        /// <param name="coordinateSystem">The coordinate system.</param>
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

        /// <summary>
        /// Sets the coordinate system by the given SRID.
        /// </summary>
        /// <param name="srid">The SRID.</param>
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
}
