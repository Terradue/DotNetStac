// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSummaryRangeObject.cs

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
#pragma warning disable SA1649 // File name should match first type name
    /// <summary>
    /// Class representing a summary <seealso href="https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md#range-object">Range Object</seealso>.
    /// </summary>
    /// <typeparam name="T">Type of the ordinal value that is summarized</typeparam>
    public class StacSummaryRangeObject<T> : StacSummaryItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StacSummaryRangeObject{T}"/> class.
        /// </summary>
        /// <param name="summary">JSON Range object</param>
        /// <exception cref="ArgumentException">Thrown when neither "minimum" nor "maximum" fields are present in the range object.</exception>
        public StacSummaryRangeObject(JObject summary)
            : base(summary)
        {
            if (!summary.ContainsKey("minimum") || !summary.ContainsKey("maximum"))
            {
                throw new ArgumentException("summary stats must contains minimum and maximum fields");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacSummaryRangeObject{T}"/> class.
        /// Initialize a Summary Range Object with a minimum and a maximum value
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public StacSummaryRangeObject(T min, T max)
            : base(new JObject())
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Gets or sets minimum of the range
        /// </summary>
        /// <returns>Minimum of the range</returns>
        /// <value>
        /// <placeholder>Minimum of the range</placeholder>
        /// </value>
        public T Min { get => this.AsJToken["minimum"].Value<T>(); set => this.AsJToken["minimum"] = new JValue(value); }

        /// <summary>
        /// Gets or sets maximum of the range
        /// </summary>
        /// <returns>Maximum of the range</returns>
        /// <value>
        /// <placeholder>Maximum of the range</placeholder>
        /// </value>
        public T Max { get => this.AsJToken["maximum"].Value<T>(); set => this.AsJToken["maximum"] = new JValue(value); }

        /// <inheritdoc/>
        public override IEnumerable<object> Enumerate()
        {
            return new object[2] { this.Min, this.Max };
        }
    }
#pragma warning restore SA1649 // File name should match first type name
}
