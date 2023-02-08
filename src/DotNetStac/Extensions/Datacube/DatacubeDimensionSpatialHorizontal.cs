// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeDimensionSpatialHorizontal.cs

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Datacube horizontal spatial dimension
    /// </summary>
    public class DatacubeDimensionSpatialHorizontal : DatacubeDimensionSpatial
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeDimensionSpatialHorizontal"/> class.
        /// </summary>
        public DatacubeDimensionSpatialHorizontal()
            : base()
        {
            this.Axis = DatacubeAxis.x;
        }
    }
}
