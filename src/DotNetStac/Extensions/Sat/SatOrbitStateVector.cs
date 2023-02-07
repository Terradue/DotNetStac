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

        /// <summary>
        /// Gets or sets time of the state vector
        /// </summary>
        /// <value>
        /// Time of the state vector
        /// </value>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets position of the state vector
        /// </summary>
        /// <value>
        /// Position of the state vector
        /// </value>
        public double[] Position { get; set; }

        /// <summary>
        /// Gets or sets velocity of the state vector
        /// </summary>
        /// <value>
        /// Velocity of the state vector
        /// </value>
        public double[] Velocity { get; set; }

        /// <summary>
        /// Gets or sets acceleration of the state vector
        /// </summary>
        /// <value>
        /// Acceleration of the state vector
        /// </value>
        public double[] Acceleration { get; set; }
    }
}
