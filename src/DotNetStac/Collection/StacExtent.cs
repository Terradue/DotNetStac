﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacExtent.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stac.Collection
{
    /// <summary>
    /// The class describes the spatio-temporal extents of the Collection, both spatial and temporal.
    /// <seealso href="https://github.com/radiantearth/stac-spec/blob/dev/collection-spec/collection-spec.md#extent-object">STAC Extent Object</seealso>
    /// </summary>
    [JsonObject]
    public class StacExtent : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StacExtent"/> class.
        /// </summary>
        /// <param name="spatial">Spatial Extent.</param>
        /// <param name="temporal">Temporal Extent.</param>
        [JsonConstructor]
        public StacExtent(StacSpatialExtent spatial, StacTemporalExtent temporal)
        {
            this.Spatial = spatial;
            this.Temporal = temporal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacExtent"/> class.
        /// Initialize a new Stac Extent from an existing one (clone).
        /// </summary>
        /// <param name="extent">The extent.</param>
        public StacExtent(StacExtent extent)
        {
            this.Spatial = new StacSpatialExtent(extent.Spatial);
            this.Temporal = new StacTemporalExtent(extent.Temporal);
        }

        /// <summary>
        /// Gets or sets Potential <see cref="StacSpatialExtent" /> covered by the Collection.
        /// </summary>
        /// <value>The spatial extent.</value>
        [JsonProperty("spatial")]
        public StacSpatialExtent Spatial { get; set; }

        /// <summary>
        /// Gets or sets Potential <see cref="StacTemporalExtent" /> covered by the Collection.
        /// </summary>
        /// <value>The temporal extent.</value>
        [JsonProperty("temporal")]
        public StacTemporalExtent Temporal { get; set; }

        /// <summary>
        /// Create a new <see cref="StacExtent" /> from a set of <see cref="StacItem" />
        /// </summary>
        /// <param name="items">Set of <see cref="StacItem" /></param>
        /// <returns>A <see cref="StacExtent" /> that represents the spatio-temporal extent of all the items together</returns>
        public static StacExtent Create(IEnumerable<StacItem> items)
        {
            DateTime? minDate = items.Where(i => i.DateTime != Itenso.TimePeriod.TimeInterval.Anytime).Min(i => i.DateTime.Start);
            DateTime? maxDate = items.Where(i => i.DateTime != Itenso.TimePeriod.TimeInterval.Anytime).Max(i => i.DateTime.End);
            minDate = minDate == DateTime.MinValue ? null : minDate;
            maxDate = maxDate == DateTime.MaxValue ? null : maxDate;
            return new StacExtent(
                new StacSpatialExtent(
                    items.Min(i => i.GetBoundingBoxFromGeometryExtent()[0]),
                    items.Min(i => i.GetBoundingBoxFromGeometryExtent()[1]),
                    items.Max(i => i.GetBoundingBoxFromGeometryExtent()[2]),
                    items.Max(i => i.GetBoundingBoxFromGeometryExtent()[3])),
                new StacTemporalExtent(minDate, maxDate));
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new StacExtent(this);
        }

        internal void Update(ICollection<StacItem> items)
        {
            this.Spatial = new StacSpatialExtent(
                items.Select(i => i.GetBoundingBoxFromGeometryExtent()[0])
                                                 .Concat(new double[] { this.Spatial.BoundingBoxes[0][0] })
                                                 .Min(),
                items.Select(i => i.GetBoundingBoxFromGeometryExtent()[1])
                                                 .Concat(new double[] { this.Spatial.BoundingBoxes[0][1] })
                                                 .Min(),
                items.Select(i => i.GetBoundingBoxFromGeometryExtent()[2])
                                                 .Concat(new double[] { this.Spatial.BoundingBoxes[0][2] })
                                                 .Max(),
                items.Select(i => i.GetBoundingBoxFromGeometryExtent()[3])
                                                 .Concat(new double[] { this.Spatial.BoundingBoxes[0][3] })
                                                 .Max());
            this.Temporal = new StacTemporalExtent(
                items.Select(i => i.DateTime.Start)
                                                   .Concat(new DateTime[] { this.Temporal.Interval[0][0].GetValueOrDefault() })
                                                   .Min(),
                items.Select(i => i.DateTime.End)
                                                   .Concat(new DateTime[] { this.Temporal.Interval[0][1].GetValueOrDefault() })
                                                   .Max());
        }
    }
}
