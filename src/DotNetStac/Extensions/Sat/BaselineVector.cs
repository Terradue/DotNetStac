// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: BaselineVector.cs

namespace Stac.Extensions.Sat
{
    public struct BaselineVector
    {
        private readonly double _perpendicular, _parallel, _along;

        public BaselineVector(double perpendicular, double parallel, double along)
        {
            this._perpendicular = perpendicular;
            this._parallel = parallel;
            this._along = along;
        }

        public double Perpendicular
        {
            get { return this._perpendicular; }
        }

        public double Parallel
        {
            get { return this._parallel; }
        }

        public double Along
        {
            get { return this._along; }
        }
    }
}
