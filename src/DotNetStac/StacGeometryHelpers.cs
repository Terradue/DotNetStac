using System.Collections;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;

namespace Stac
{
    public static class StacGeometryHelpers
    {

        public static IPosition[] GetBoundingBox(this IGeometryObject geometry)
        {
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

        public static IPosition GetLowerLeft(this IPosition[] positions)
        {
            if (positions == null || positions.Length == 0) return null;
            if (positions[0].Altitude.HasValue)
                return new GeoJSON.Net.Geometry.Position(positions.Min(p => p.Latitude), positions.Min(p => p.Longitude), positions.Min(p => p.Altitude));
            else 
                return new GeoJSON.Net.Geometry.Position(positions.Min(p => p.Latitude), positions.Min(p => p.Longitude));
        }

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
