// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: BaselineCalculation.cs

using System;
using GeoJSON.Net.Geometry;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Stac.Extensions.Sat
{
    /// <summary>
    /// Baseline calculation helper class.
    /// </summary>
    public class BaselineCalculation
    {
        /// <summary>Synchronizes the vector.</summary>
        /// <returns>The vector.</returns>
        /// <param name="t0">T0.</param>
        /// <param name="groundPoint">Ground point.</param>
        /// <param name="slave">Slave.</param>
        /// <param name="converged">Converged.</param>
        /// <param name="maxError">Max error.</param>
        /// <param name="maxIterations">Max iterations.</param>
        /// <param name="tolerance">Tolerance.</param>
        public static double SynchronizeVector(
            double t0,
            IPosition groundPoint,
            IInterpolation[] slave,
            out bool converged,
            out double maxError,
            int maxIterations = 100,
            double tolerance = 0.00000001)
        {
            double t = t0;
            double delta = 0;

            for (int i = 0; i < maxIterations; i++)
            {
                double x = groundPoint.Longitude;
                double y = groundPoint.Latitude;
                double z = groundPoint.Altitude.Value;

                double sxm = Sx(t, slave);
                double sym = Sy(t, slave);
                double szm = Sz(t, slave);

                double vxm = Vx(t, slave);
                double vym = Vy(t, slave);
                double vzm = Vz(t, slave);

                double axm = Ax(t, slave);
                double aym = Ay(t, slave);
                double azm = Az(t, slave);

                delta = -((vxm * (x - sxm)) + (vym * (y - sym)) + (vzm * (z - szm))) / ((axm * (x - sxm)) + (aym * (y - sym)) + (azm * (z - szm)) - (vxm * vxm) - (vym * vym) - (vzm * vzm));
                t += delta;
                if (Math.Abs(delta) < tolerance)
                {
                    break;
                }
            }

            maxError = Math.Abs(delta);
            converged = true;

            Console.WriteLine("RESULT: {0}", t);

            return t;
        }

        /// <summary>Synchronizes the vector2.</summary>
        /// <param name="t0">T0.</param>
        /// <param name="groundPoint">Ground point.</param>
        /// <param name="interpol">Interpol.</param>
        /// <param name="converged">Converged.</param>
        /// <param name="maxError">Max error.</param>
        /// <param name="maxIterations">Max iterations.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <returns>The vector2.</returns>
        public static double SynchronizeVector2(
            double t0,
            IPosition groundPoint,
            IInterpolation[] interpol,
            out bool converged,
            out double maxError,
            int maxIterations = 100,
            double tolerance = 0.00000001)
        {
            double t = t0;
            double delta = 0;

            for (int i = 0; i < maxIterations; i++)
            {
                double x = groundPoint.Longitude;
                double y = groundPoint.Latitude;
                double z = groundPoint.Altitude.Value;

                double sxm = Sx(t, interpol);
                double sym = Sy(t, interpol);
                double szm = Sz(t, interpol);

                double vxm = Vx(t, interpol);
                double vym = Vy(t, interpol);
                double vzm = Vz(t, interpol);

                double axm = Ax(t, interpol);
                double aym = Ay(t, interpol);
                double azm = Az(t, interpol);

                delta = -((vxm * (x - sxm)) + (vym * (y - sym)) + (vzm * (z - szm))) / ((axm * (x - sxm)) + (aym * (y - sym)) + (azm * (z - szm)) - (vxm * vxm) - (vym * vym) - (vzm * vzm));
                t += delta;
                if (Math.Abs(delta) < tolerance)
                {
                    break;
                }
            }

            maxError = Math.Abs(delta);
            converged = true;

            Console.WriteLine("RESULT: {0}", t);

            return t;
        }

        /// <summary>Calculates the baseline.</summary>
        /// <param name="time">Time.</param>
        /// <param name="master">Master.</param>
        /// <param name="slave">Slave.</param>
        /// <param name="groundPoint">Ground point.</param>
        /// <returns>The baseline.</returns>
        public static BaselineVector CalculateBaseline(DateTime[] time, SatOrbitStateVector[] master, SatOrbitStateVector[] slave, IPosition groundPoint)
        {
            var interpolSlave = PolyInterpol(time[1], slave);
            var interpolMaster = PolyInterpol(time[0], master);

            double x = groundPoint.Longitude;
            double y = groundPoint.Latitude;
            double z = groundPoint.Altitude.Value;

            // bool converged;
            // double maxError;
            double t = time[2].Subtract(time[0]).TotalSeconds;

            // double tm = SynchronizeVector(t, groundPoint, interpolMaster, out converged, out maxError);
            // double ts = SynchronizeVector(t, groundPoint, interpolSlave, out converged, out maxError);

            // Define baseline vector:
            double[] defaultBaselineVector = new double[3]
            {
                Sx(t, interpolSlave) - Sx(t, interpolMaster),
                Sy(t, interpolSlave) - Sy(t, interpolMaster),
                Sz(t, interpolSlave) - Sz(t, interpolMaster),
            };

            double baselineModulo = Math.Sqrt((defaultBaselineVector[0] * defaultBaselineVector[0]) + (defaultBaselineVector[1] * defaultBaselineVector[1]) + (defaultBaselineVector[2] * defaultBaselineVector[2]));

            // Baseline unit vector
            double[] baselineUnit = new double[3];
            for (int k = 0; k < 3; k++)
            {
                baselineUnit[k] = defaultBaselineVector[k] / baselineModulo;
            }

            // Parallel vector
            double[] parallelVector = new double[3]
            {
                x - Sx(t, interpolMaster),
                y - Sy(t, interpolMaster),
                z - Sz(t, interpolMaster),
            };
            double parallelVectorModulo = Math.Sqrt((parallelVector[0] * parallelVector[0]) + (parallelVector[1] * parallelVector[1]) + (parallelVector[2] * parallelVector[2]));

            // Parallel unit vector
            double[] parallelVectorUnit = new double[3];
            for (int k = 0; k < 3; k++)
            {
                parallelVectorUnit[k] = parallelVector[k] / parallelVectorModulo;
            }

            // Along-track vector
            double[] alongTrackVector = new double[3]
            {
                Vx(t, interpolMaster),
                Vy(t, interpolMaster),
                Vz(t, interpolMaster),
            };

            double alongTrackVectorMod = Math.Sqrt((alongTrackVector[0] * alongTrackVector[0]) + (alongTrackVector[1] * alongTrackVector[1]) + (alongTrackVector[2] * alongTrackVector[2]));

            // Along-track unit vector
            double[] alongTrackVectorUnit = new double[3];
            for (int k = 0; k < 3; k++)
            {
                alongTrackVectorUnit[k] = alongTrackVector[k] / alongTrackVectorMod;
            }

            double[] perpendicularVectorUnit = VectorProduct3D(parallelVectorUnit, alongTrackVectorUnit);

            double perpendicularBaseline = (defaultBaselineVector[0] * perpendicularVectorUnit[0]) + (defaultBaselineVector[1] * perpendicularVectorUnit[1]) + (defaultBaselineVector[2] * perpendicularVectorUnit[2]);
            double parallelBaseline = (defaultBaselineVector[0] * parallelVectorUnit[0]) + (defaultBaselineVector[1] * parallelVectorUnit[1]) + (defaultBaselineVector[2] * parallelVectorUnit[2]);
            double alongBaseline = (defaultBaselineVector[0] * alongTrackVectorUnit[0]) + (defaultBaselineVector[1] * alongTrackVectorUnit[1]) + (defaultBaselineVector[2] * alongTrackVectorUnit[2]);

            return new BaselineVector(perpendicularBaseline, parallelBaseline, alongBaseline);
        }

        /// <summary>Calculates 3D vector product</summary>
        /// <param name="vectorA">Vector A.</param>
        /// <param name="vectorB">Vector B.</param>
        /// <returns>3D vector product.</returns>
        public static double[] VectorProduct3D(double[] vectorA, double[] vectorB)
        {
            if (vectorA.Length != 3 || vectorB.Length != 3)
            {
                throw new ArgumentException("3D vector product cannot be calculated, because one of the vectors is not a 3D vector");
            }

            return new double[3]
            {
                (vectorA[1] * vectorB[2]) - (vectorA[2] * vectorB[1]),
                (vectorA[2] * vectorB[0]) - (vectorA[0] * vectorB[2]),
                (vectorA[0] * vectorB[1]) - (vectorA[1] * vectorB[0]),
            };
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Sx(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 0);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Sy(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 1);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Sz(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 2);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Vx(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 3);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Vy(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 4);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Vz(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 5);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Ax(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 6);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Ay(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 7);
        }

        /// <summary>Calculates the orbit value.</summary>
        /// <param name="time">Time.</param>
        /// <param name="orbitStates">Orbit states.</param>
        /// <returns>The orbit value.</returns>
        public static double Az(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 8);
        }

        /// <summary>
        /// Interpolates the orbit state vectors.
        /// </summary>
        /// <param name="time0">Time of the orbit state vector to interpolate</param>
        /// <param name="orbitStateVectors">The orbit state vectors.</param>
        /// <returns>Interpolated orbit state vectors.</returns>
        public static IInterpolation[] PolyInterpol(DateTime time0, SatOrbitStateVector[] orbitStateVectors)
        {
            var time = Array.ConvertAll(orbitStateVectors, o => o.Time.Subtract(time0).TotalSeconds);

            Vector<double> x1 = DenseVector.OfArray(Array.ConvertAll(orbitStateVectors, o => o.Position[0]));
            Vector<double> y1 = DenseVector.OfArray(Array.ConvertAll(orbitStateVectors, o => o.Position[1]));
            Vector<double> z1 = DenseVector.OfArray(Array.ConvertAll(orbitStateVectors, o => o.Position[2]));

            Vector<double> vx1 = DenseVector.OfArray(Array.ConvertAll(orbitStateVectors, o => o.Velocity[0]));
            Vector<double> vy1 = DenseVector.OfArray(Array.ConvertAll(orbitStateVectors, o => o.Velocity[1]));
            Vector<double> vz1 = DenseVector.OfArray(Array.ConvertAll(orbitStateVectors, o => o.Velocity[2]));

            Vector<double> ax1 = DenseVector.Create(vx1.Count, 0.0);
            Vector<double> ay1 = DenseVector.Create(vy1.Count, 0.0);
            Vector<double> az1 = DenseVector.Create(vz1.Count, 0.0);
            for (int i = 0; i < vx1.Count; i++)
            {
                ax1[i] = (vx1[i] * i) + 1;
                ay1[i] = (vy1[i] * i) + 1;
                az1[i] = (vz1[i] * i) + 1;
            }

            IInterpolation[] coeff = new IInterpolation[9];

            coeff[0] = Interpolate.CubicSplineRobust(time, x1.ToArray());
            coeff[1] = Interpolate.CubicSplineRobust(time, y1.ToArray());
            coeff[2] = Interpolate.CubicSplineRobust(time, z1.ToArray());
            coeff[3] = Interpolate.CubicSplineRobust(time, vx1.ToArray());
            coeff[4] = Interpolate.CubicSplineRobust(time, vy1.ToArray());
            coeff[5] = Interpolate.CubicSplineRobust(time, vz1.ToArray());
            coeff[6] = Interpolate.CubicSplineRobust(time, ax1.ToArray());
            coeff[7] = Interpolate.CubicSplineRobust(time, ay1.ToArray());
            coeff[8] = Interpolate.CubicSplineRobust(time, az1.ToArray());

            return coeff;
        }

        private static double GetOrbitValue(double time, IInterpolation[] interpol, int index)
        {
            return interpol[index].Interpolate(time);
        }
    }
}
