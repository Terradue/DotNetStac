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
    public class BaselineCalculation
    {
        /// <summary>Synchronizes the vector.</summary>
        /// <returns>The vector.</returns>
        /// <param name="t0">T0.</param>
        /// <param name="groundPoint">Ground point.</param>
        /// <param name="slave"></param>
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
            double[] b = new double[3]
            {
                Sx(t, interpolSlave) - Sx(t, interpolMaster),
                Sy(t, interpolSlave) - Sy(t, interpolMaster),
                Sz(t, interpolSlave) - Sz(t, interpolMaster),
            };

            double bMod = Math.Sqrt((b[0] * b[0]) + (b[1] * b[1]) + (b[2] * b[2]));

            // Baseline unit vector
            double[] bUnit = new double[3]; //
            for (int k = 0; k < 3; k++)
            {
                bUnit[k] = b[k] / bMod;
            }

            // Parallel vector
            double[] vpar = new double[3]
            {
                x - Sx(t, interpolMaster),
                y - Sy(t, interpolMaster),
                z - Sz(t, interpolMaster),
            };
            double vparMod = Math.Sqrt((vpar[0] * vpar[0]) + (vpar[1] * vpar[1]) + (vpar[2] * vpar[2]));

            // Parallel unit vector
            double[] vparUnit = new double[3];
            for (int k = 0; k < 3; k++)
            {
                vparUnit[k] = vpar[k] / vparMod;
            }

            // Along-track vector
            double[] va = new double[3]
            {
                Vx(t, interpolMaster),
                Vy(t, interpolMaster),
                Vz(t, interpolMaster),
            };

            double vaMod = Math.Sqrt((va[0] * va[0]) + (va[1] * va[1]) + (va[2] * va[2]));

            // Along-track unit vector
            double[] vaUnit = new double[3];
            for (int k = 0; k < 3; k++)
            {
                vaUnit[k] = va[k] / vaMod;
            }

            double[] vperpUnit = VectorProduct3D(vparUnit, vaUnit);

            double bPerp = (b[0] * vperpUnit[0]) + (b[1] * vperpUnit[1]) + (b[2] * vperpUnit[2]);
            double bParallel = (b[0] * vparUnit[0]) + (b[1] * vparUnit[1]) + (b[2] * vparUnit[2]);
            double bAlong = (b[0] * vaUnit[0]) + (b[1] * vaUnit[1]) + (b[2] * vaUnit[2]);

            return new BaselineVector(bPerp, bParallel, bAlong);
        }

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

        public static double Sx(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 0);
        }

        public static double Sy(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 1);
        }

        public static double Sz(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 2);
        }

        public static double Vx(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 3);
        }

        public static double Vy(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 4);
        }

        public static double Vz(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 5);
        }

        public static double Ax(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 6);
        }

        public static double Ay(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 7);
        }

        public static double Az(double time, IInterpolation[] orbitStates)
        {
            return GetOrbitValue(time, orbitStates, 8);
        }

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
