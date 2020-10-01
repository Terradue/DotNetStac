using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Stac.Item;

namespace Stac
{
    public interface IStacItem : IStacObject
    {
        IDictionary<string, StacAsset> Assets { get; }

        Itenso.TimePeriod.ITimePeriod DateTime { get; }

        IGeometryObject Geometry { get; }

        double[] BoundingBoxes { get; }
    }
}