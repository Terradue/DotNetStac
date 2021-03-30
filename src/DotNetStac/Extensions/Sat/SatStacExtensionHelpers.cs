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
        public static BaselineVector CalculateBaseline(this SatStacExtension sat1, SatStacExtension sat2)
        {
            if (!(sat1.StacItem is StacItem))
                throw new OperationCanceledException(string.Format("{0} must be an item to calculate baseline", sat1.StacItem.Id));
            if (!(sat2.StacItem is StacItem))
                throw new OperationCanceledException(string.Format("{0} must be an item to calculate baseline", sat1.StacItem.Id));

            StacItem masterItem = sat1.StacItem as StacItem;
            StacItem slaveItem = sat2.StacItem as StacItem;

            if (sat1.OrbitStateVectors.Count() == 0)
                throw new OperationCanceledException("sat1 has no orbit state vectors");
            if (sat2.OrbitStateVectors.Count() == 0)
                throw new OperationCanceledException("sat2 has no orbit state vectors");

            DateTime masterAnxDate = sat1.AscendingNodeCrossingDateTime;
            DateTime slaveAnxDate = sat2.AscendingNodeCrossingDateTime;

            var masterOrbits = sat1.OrbitStateVectors.Values;
            var slaveOrbits = sat2.OrbitStateVectors.Values;

            IPosition p0 = new Position(sat1.SceneCenterCoordinates[0], sat1.SceneCenterCoordinates[1], sat1.SceneCenterCoordinates[2]);
            DateTime[] times = new DateTime[3] { masterAnxDate, slaveAnxDate, masterItem.DateTime.Start.Add(TimeSpan.FromMilliseconds(masterItem.DateTime.End.Subtract(masterItem.DateTime.Start).TotalMilliseconds / 2)) };

            var baseline = BaselineCalculation.CalculateBaseline(times, masterOrbits.ToArray(), slaveOrbits.ToArray(), p0);

            return baseline;

        }
    }
}
