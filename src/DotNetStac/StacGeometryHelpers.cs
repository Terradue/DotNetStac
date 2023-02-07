// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacGeometryHelpers.cs

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
        public static double[] GetBoundingBoxFromGeometryExtent(this StacItem stacItem)
        {
            var boundingBoxes = stacItem.Geometry?.GetBoundingBox();
            if (boundingBoxes == null)
            {
                return new double[] { -180, -90, 180, 90 };
            }

            if (boundingBoxes[0].Altitude.HasValue)
                return new double[]
                {
                    boundingBoxes[0].Longitude, boundingBoxes[0].Latitude, boundingBoxes[0].Altitude.Value,
                    boundingBoxes[1].Longitude, boundingBoxes[1].Latitude, boundingBoxes[1].Altitude.Value,
                };
            else
                return new double[]
                {
                    boundingBoxes[0].Longitude, boundingBoxes[0].Latitude,
                    boundingBoxes[1].Longitude, boundingBoxes[1].Latitude,
                };
        }

        /// <summary>
        /// Get the bounding box of a geometry object
        /// </summary>
        /// <param name="geometry">the geometry object</param>
        public static IPosition[] GetBoundingBox(this IGeometryObject geometry)
        {
            if (geometry == null)
            {
                throw new ArgumentNullException(nameof(geometry));
            }

            IPosition upperRight = null, lowerLeft = null;
            double lowerLeftLon, lowerLeftLat, upperRightLon, upperRightLat;
            double? lowerLeftAlt = null, upperRightAlt = null;

            switch (geometry.Type)
            {
                case GeoJSONObjectType.GeometryCollection:
                    GeometryCollection geometryCollection = geometry as GeometryCollection;
                    lowerLeftLon = geometryCollection.Geometries.Min(gp => gp.GetBoundingBox()[0].Longitude);
                    lowerLeftLat = geometryCollection.Geometries.Min(gp => gp.GetBoundingBox()[0].Latitude);
                    lowerLeftAlt = geometryCollection.Geometries.Min(gp => gp.GetBoundingBox()[0].Altitude);
                    upperRightLon = geometryCollection.Geometries.Max(gp => gp.GetBoundingBox()[1].Longitude);
                    upperRightLat = geometryCollection.Geometries.Max(gp => gp.GetBoundingBox()[1].Latitude);
                    upperRightAlt = geometryCollection.Geometries.Max(gp => gp.GetBoundingBox()[1].Altitude);
                    lowerLeft = new Position(lowerLeftLat, lowerLeftLon, lowerLeftAlt);
                    upperRight = new Position(upperRightLat, upperRightLon, upperRightAlt);
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    MultiPolygon multiPolygon = geometry as MultiPolygon;
                    lowerLeftLon = multiPolygon.Coordinates.Min(gp => gp.GetBoundingBox()[0].Longitude);
                    lowerLeftLat = multiPolygon.Coordinates.Min(gp => gp.GetBoundingBox()[0].Latitude);
                    lowerLeftAlt = multiPolygon.Coordinates.Min(gp => gp.GetBoundingBox()[0].Altitude);
                    upperRightLon = multiPolygon.Coordinates.Max(gp => gp.GetBoundingBox()[1].Longitude);
                    upperRightLat = multiPolygon.Coordinates.Max(gp => gp.GetBoundingBox()[1].Latitude);
                    upperRightAlt = multiPolygon.Coordinates.Max(gp => gp.GetBoundingBox()[1].Altitude);
                    lowerLeft = new Position(lowerLeftLat, lowerLeftLon, lowerLeftAlt);
                    upperRight = new Position(upperRightLat, upperRightLon, upperRightAlt);
                    break;
                case GeoJSONObjectType.MultiLineString:
                    MultiLineString multiLineString = geometry as MultiLineString;
                    lowerLeftLon = multiLineString.Coordinates.Min(gp => gp.GetBoundingBox()[0].Longitude);
                    lowerLeftLat = multiLineString.Coordinates.Min(gp => gp.GetBoundingBox()[0].Latitude);
                    lowerLeftAlt = multiLineString.Coordinates.Min(gp => gp.GetBoundingBox()[0].Altitude);
                    upperRightLon = multiLineString.Coordinates.Max(gp => gp.GetBoundingBox()[1].Longitude);
                    upperRightLat = multiLineString.Coordinates.Max(gp => gp.GetBoundingBox()[1].Latitude);
                    upperRightAlt = multiLineString.Coordinates.Max(gp => gp.GetBoundingBox()[1].Altitude);
                    lowerLeft = new Position(lowerLeftLat, lowerLeftLon, lowerLeftAlt);
                    upperRight = new Position(upperRightLat, upperRightLon, upperRightAlt);
                    break;
                case GeoJSONObjectType.Polygon:
                    Polygon polygon = geometry as Polygon;
                    lowerLeft = polygon.Coordinates.Min(ls => ls.GetBoundingBox()[0]);
                    upperRight = polygon.Coordinates.Max(ls => ls.GetBoundingBox()[1]);
                    break;
                case GeoJSONObjectType.LineString:
                    LineString lineString = geometry as LineString;
                    lowerLeft = lineString.Coordinates.ToArray().GetLowerLeft();
                    upperRight = lineString.Coordinates.ToArray().GetUpperRight();
                    break;
                case GeoJSONObjectType.MultiPoint:
                    MultiPoint multiPoint = geometry as MultiPoint;
                    lowerLeft = multiPoint.Coordinates.Select(p => p.Coordinates).ToArray().GetLowerLeft();
                    upperRight = multiPoint.Coordinates.Select(p => p.Coordinates).ToArray().GetUpperRight();
                    break;
                case GeoJSONObjectType.Point:
                    Point point = geometry as Point;
                    lowerLeft = point.Coordinates;
                    upperRight = point.Coordinates;
                    break;
            }

            return new IPosition[2] { lowerLeft, upperRight };
        }

        /// <summary>
        /// Get the lower left corner of a bounding box
        /// </summary>
        /// <param name="positions">set of positions</param>
        public static IPosition GetLowerLeft(this IPosition[] positions)
        {
            if (positions == null || positions.Length == 0)
            {
                return null;
            }

            if (positions[0].Altitude.HasValue)
            {
                return new Position(positions.Min(p => p.Latitude), positions.Min(p => p.Longitude), positions.Min(p => p.Altitude));
            }
            else
            {
                return new Position(positions.Min(p => p.Latitude), positions.Min(p => p.Longitude));
            }
        }

        /// <summary>
        /// Get the upper right corner of a bounding box.
        /// </summary>
        /// <param name="positions">set of positions.</param>
        public static IPosition GetUpperRight(this IPosition[] positions)
        {
            if (positions == null || positions.Length == 0)
            {
                return null;
            }

            if (positions[0].Altitude.HasValue)
            {
                return new Position(positions.Max(p => p.Latitude), positions.Max(p => p.Longitude), positions.Max(p => p.Altitude));
            }
            else
            {
                return new Position(positions.Max(p => p.Latitude), positions.Max(p => p.Longitude));
            }
        }
    }
}
