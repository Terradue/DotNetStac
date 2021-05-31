using System;

namespace Stac.Extensions.Sat
{

    public struct BaselineVector
    {
        private double perpendicular, parallel, along;

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
