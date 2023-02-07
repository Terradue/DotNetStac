// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: BaselineVector.cs

namespace Stac.Extensions.Sat
{

    public struct BaselineVector
    {
        private readonly double perpendicular, parallel, along;

        public double Perpendicular
        {
            get { return perpendicular; }
        }

        public double Parallel
        {
            get { return parallel; }
        }

        public double Along
        {
            get { return along; }
        }

        public BaselineVector(double perpendicular, double parallel, double along)
        {
            this.perpendicular = perpendicular;
            this.parallel = parallel;
            this.along = along;
        }
    }

}
