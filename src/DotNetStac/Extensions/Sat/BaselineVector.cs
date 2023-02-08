// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: BaselineVector.cs

namespace Stac.Extensions.Sat
{
    /// <summary>
    /// Baseline vector
    /// </summary>
    public struct BaselineVector
    {
        private readonly double _perpendicular;
        private readonly double _parallel;
        private readonly double _along;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaselineVector"/> struct.
        /// </summary>
        /// <param name="perpendicular">Perpendicular component.</param>
        /// <param name="parallel">Parallel component.</param>
        /// <param name="along">Along component.</param>
        public BaselineVector(double perpendicular, double parallel, double along)
        {
            this._perpendicular = perpendicular;
            this._parallel = parallel;
            this._along = along;
        }

        /// <summary>
        /// Gets the perpendicular component.
        /// </summary>
        /// <returns>The perpendicular component.</returns>
        /// <value>
        /// The perpendicular component.
        /// </value>
        public double Perpendicular
        {
            get { return this._perpendicular; }
        }

        /// <summary>
        /// Gets the parallel component.
        /// </summary>
        /// <returns>The parallel component.</returns>
        /// <value>
        /// The parallel component.
        /// </value>
        public double Parallel
        {
            get { return this._parallel; }
        }

        /// <summary>
        /// Gets the along component.
        /// </summary>
        /// <returns>The along component.</returns>
        /// <value>
        /// The along component.
        /// </value>
        public double Along
        {
            get { return this._along; }
        }
    }
}
