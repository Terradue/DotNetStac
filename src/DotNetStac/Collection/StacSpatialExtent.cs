﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSpatialExtent.cs

using System;
using Newtonsoft.Json;

namespace Stac.Collection
{
    /// <summary>
    /// The class represents the spatial extents.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/collection-spec/collection-spec.md#spatial-extent-object">Spatial Extent Object</seealso>
    /// </summary>
    [JsonObject]
    public class StacSpatialExtent : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StacSpatialExtent"/> class.
        /// </summary>
        /// <param name="minX">Minimum X bound</param>
        /// <param name="minY">Minimum Y bound</param>
        /// <param name="maxX">Maximum X bound</param>
        /// <param name="maxY">Maximum Y bound</param>
        [JsonConstructor]
        public StacSpatialExtent(double minX, double minY, double maxX, double maxY)
        {
            this.BoundingBoxes = new double[1][] { new double[4] { minX, minY, maxX, maxY } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacSpatialExtent"/> class.
        /// Initialize a new Stac Spatial extent from an existing one (clone)
        /// </summary>
        /// <param name="spatial">The spatial extent.</param>
        public StacSpatialExtent(StacSpatialExtent spatial)
        {
            this.BoundingBoxes = (double[][])spatial.BoundingBoxes.Clone();
        }

        /// <summary>
        /// Gets or sets potential spatial extents.
        /// </summary>
        /// <value>Gets/sets double entry array of coordinates</value>
        [JsonProperty("bbox")]
        public double[][] BoundingBoxes { get; set; }

        /// <summary>
        /// Clone this Extent
        /// </summary>
        /// <returns>A new <see cref="StacSpatialExtent" /> that is a clone of this instance.</returns>
        public object Clone()
        {
            return new StacSpatialExtent(this);
        }
    }
}
