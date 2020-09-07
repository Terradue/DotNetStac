using System;
using System.Linq;
using GeoJSON.Net.Geometry;
using Itenso.TimePeriod;
using Stac;
using Stac.Extensions;

namespace Stac.Extensions.Sat
{
    public static class SatStacExtensionHelpers
    {
        public static double CalculateBaseline(this SatStacExtension sat1, SatStacExtension sat2)
        {
            if (sat1.StacObject.IsCatalog)
                throw new OperationCanceledException(string.Format("{0} must be an item to calculate baseline", sat1.StacObject.Id));
            if (sat2.StacObject.IsCatalog)
                throw new OperationCanceledException(string.Format("{0} must be an item to calculate baseline", sat1.StacObject.Id));

            IStacItem masterItem = sat1.StacObject as IStacItem;
            IStacItem slaveItem = sat2.StacObject as IStacItem;

            if (sat1.OrbitStateVectors.Count() == 0)
                throw new OperationCanceledException("sat1 has no orbit state vectors");
            if (sat2.OrbitStateVectors.Count() == 0)
                throw new OperationCanceledException("sat2 has no orbit state vectors");

            DateTime masterAnxDate = sat1.AscendingNodeDateTime;
            DateTime slaveAnxDate = sat2.AscendingNodeDateTime;

            var masterOrbits = sat1.OrbitStateVectors.Values;
            var slaveOrbits = sat2.OrbitStateVectors.Values;

            IPosition p0 = new Position(sat1.SceneCenterCoordinates[0], sat1.SceneCenterCoordinates[1], sat1.SceneCenterCoordinates[2]);
            DateTime[] times = new DateTime[3] { masterAnxDate, slaveAnxDate, masterItem.DateTime.Start.Add(TimeSpan.FromMilliseconds(masterItem.DateTime.End.Subtract(masterItem.DateTime.Start).TotalMilliseconds / 2)) };

            var baseline = BaselineCalculation.CalculateBaseline(times, masterOrbits.ToArray(), slaveOrbits.ToArray(), p0);

            return baseline.Perpendicular;

        }
    }
}