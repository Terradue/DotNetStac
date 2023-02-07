// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SatOrbitStateVector.cs

using System;

namespace Stac.Extensions.Sat
{
    public class SatOrbitStateVector
    {
        public DateTime Time;

        public double[] Position;

        public double[] Velocity;

        public double[] Acceleration;

        public SatOrbitStateVector(DateTime time, double[] position, double[] velocity, double[] acceleration)
        {
            Time = time;
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
        }
    }
}
