using System;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;

namespace Stac
{
    /// <summary>
    /// Some helpers for manipulating geometries
    /// </summary>
    public static class StacGeometryHelpers
    {

        /// <summary>
        /// Get The bounding box of a geometry in a StacItem
        /// </summary>
        /// <param name="stacItem">The STAC Item containing the geometry</param>
        /// <returns></returns>
        public static double[] GetBoundingBoxFromGeometryExtent(this StacItem stacItem)
        {
            var boundingBoxes = stacItem.Geometry.GetBoundingBox();
            if (boundingBoxes[0].Altitude.HasValue)
                return new double[] {
                    boundingBoxes[0].Longitude, boundingBoxes[0].Latitude, boundingBoxes[0].Altitude.Value,
                    boundingBoxes[1].Longitude, boundingBoxes[1].Latitude, boundingBoxes[1].Altitude.Value,
                };
            else
                return new double[] {
                    boundingBoxes[0].Longitude, boundingBoxes[0].Latitude,
                    boundingBoxes[1].Longitude, boundingBoxes[1].Latitude,
                };
        }

        /// <summary>
        /// Get the bounding box of a geometry object
        /// </summary>
        /// <param name="geometry">the geometry object</param>
        /// <returns></returns>
        public static IPosition[] GetBoundingBox(this IGeometryObject geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));
            IPosition upperRight = null, lowerLeft = null;

            switch (geometry.Type)
            {
                case GeoJSONObjectType.GeometryCollection:
                    GeometryCollection geometryCollection = geometry as GeometryCollection;
                    lowerLeft = geometryCollection.Geometries.Min(geom => geom.GetBoundingBox()[0]);
                    upperRight = geometryCollection.Geometries.Max(geom => geom.GetBoundingBox()[1]);
                    break;
                case GeoJSONObjectType.LineString:
                    LineString lineString = geometry as LineString;
                    lowerLeft = lineString.Coordinates.ToArray().GetLowerLeft();
                    upperRight = lineString.Coordinates.ToArray().GetUpperRight();
                    break;
                case GeoJSONObjectType.MultiLineString:
                    MultiLineString multiLineString = geometry as MultiLineString;
                    lowerLeft = multiLineString.Coordinates.Min(lString => lString.GetBoundingBox()[0]);
                    upperRight = multiLineString.Coordinates.Max(lString => lString.GetBoundingBox()[1]);
                    break;
                case GeoJSONObjectType.MultiPoint:
                    MultiPoint multiPoint = geometry as MultiPoint;
                    lowerLeft = multiPoint.Coordinates.Select(p => p.Coordinates).ToArray().GetLowerLeft();
                    upperRight = multiPoint.Coordinates.Select(p => p.Coordinates).ToArray().GetUpperRight();
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    MultiPolygon multiPolygon = geometry as MultiPolygon;
                    lowerLeft = multiPolygon.Coordinates.Min(poly => poly.GetBoundingBox()[0]);
                    upperRight = multiPolygon.Coordinates.Max(poly => poly.GetBoundingBox()[1]);
                    break;
                case GeoJSONObjectType.Point:
                    Point point = geometry as Point;
                    lowerLeft = point.Coordinates;
                    upperRight = point.Coordinates;
                    break;
                case GeoJSONObjectType.Polygon:
                    Polygon polygon = geometry as Polygon;
                    lowerLeft = polygon.Coordinates.Min(lString => lString.GetBoundingBox()[0]);
                    upperRight = polygon.Coordinates.Max(lString => lString.GetBoundingBox()[1]);
                    break;
            }

            return new IPosition[2] { lowerLeft, upperRight };
        }

        /// <summary>
        /// Get the lower left corner of a bounding box
        /// </summary>
        /// <param name="positions">set of positions</param>
        /// <returns></returns>
        public static IPosition GetLowerLeft(this IPosition[] positions)
        {
            if (positions == null || positions.Length == 0) return null;
            if (positions[0].Altitude.HasValue)
                return new GeoJSON.Net.Geometry.Position(positions.Min(p => p.Latitude), positions.Min(p => p.Longitude), positions.Min(p => p.Altitude));
            else
                return new GeoJSON.Net.Geometry.Position(positions.Min(p => p.Latitude), positions.Min(p => p.Longitude));
        }

        /// <summary>
        /// Get the upper right corner of a bounding box
        /// </summary>
        /// <param name="positions">set of positions</param>
        /// <returns></returns>
        public static IPosition GetUpperRight(this IPosition[] positions)
        {
            if (positions == null || positions.Length == 0) return null;
            if (positions[0].Altitude.HasValue)
                return new GeoJSON.Net.Geometry.Position(positions.Max(p => p.Latitude), positions.Max(p => p.Longitude), positions.Max(p => p.Altitude));
            else
                return new GeoJSON.Net.Geometry.Position(positions.Max(p => p.Latitude), positions.Max(p => p.Longitude));
        }
    }
}
