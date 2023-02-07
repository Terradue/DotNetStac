// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SatStacExtensionHelpers.cs

using System;
using System.Linq;
using GeoJSON.Net.Geometry;

namespace Stac.Extensions.Sat
{
    /// <summary>
    /// Helpers methods for the SatStacExtension
    /// </summary>
    public static class SatStacExtensionHelpers
    {
        /// <summary>
        /// Calculates the baseline.
        /// </summary>
        /// <returns>The baseline.</returns>
        /// <param name="sat1">Sat1.</param>
        /// <param name="sat2">Sat2.</param>
        public static BaselineVector CalculateBaseline(this SatStacExtension sat1, SatStacExtension sat2)
        {
            StacItem masterItem = sat1.StacItem;
            StacItem slaveItem = sat2.StacItem;

            if (sat1.OrbitStateVectors.Count() == 0)
            {
                throw new OperationCanceledException("sat1 has no orbit state vectors");
            }

            if (sat2.OrbitStateVectors.Count() == 0)
            {
                throw new OperationCanceledException("sat2 has no orbit state vectors");
            }

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
