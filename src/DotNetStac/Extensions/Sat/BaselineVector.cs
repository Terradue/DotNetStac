// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: BaselineVector.cs

namespace Stac.Extensions.Sat
{

    public struct BaselineVector
    {
        private readonly double perpendicular, parallel, along;

        public BaselineVector(double perpendicular, double parallel, double along)
        {
            this.perpendicular = perpendicular;
            this.parallel = parallel;
            this.along = along;
        }

        public double Perpendicular
        {
            get { return this.perpendicular; }
        }

        public double Parallel
        {
            get { return this.parallel; }
        }

        public double Along
        {
            get { return this.along; }
        }
    }
}
