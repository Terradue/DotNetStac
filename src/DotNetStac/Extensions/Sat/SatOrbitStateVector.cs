// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SatOrbitStateVector.cs

using System;

namespace Stac.Extensions.Sat
{
    /// <summary>
    /// Orbit state vector
    /// </summary>
    public class SatOrbitStateVector
    {
        /// <summary>
        /// Time of the state vector
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// Position of the state vector
        /// </summary>
        public double[] Position;

        /// <summary>
        /// Velocity of the state vector
        /// </summary>
        public double[] Velocity;

        /// <summary>
        /// Acceleration of the state vector
        /// </summary>
        public double[] Acceleration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SatOrbitStateVector"/> class.
        /// </summary>
        /// <param name="time">Time of the state vector</param>
        /// <param name="position">Position of the state vector</param>
        /// <param name="velocity">Velocity of the state vector</param>
        /// <param name="acceleration">Acceleration of the state vector</param>
        public SatOrbitStateVector(DateTime time, double[] position, double[] velocity, double[] acceleration)
        {
            this.Time = time;
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
        }
    }
}
