// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSummaryRangeObject.cs

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    /// <summary>
    /// Class representing a summary <seealso href="https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md#range-object">Range Object</seealso>.
    /// </summary>
    /// <typeparam name="T">Type of the ordinal value that is summarized</typeparam>
    public class StacSummaryRangeObject<T> : StacSummaryItem
    {
        /// <summary>
        /// Initialize a Summary Range Object with a JSON object.
        /// </summary>
        /// <param name="summary">JSON Range object</param>
        /// <exception cref="ArgumentException">Thrown when neither "minimum" nor "maximum" fields are present in the range object.</exception>
        /// <returns></returns>
        public StacSummaryRangeObject(JObject summary) : base(summary)
        {
            if (!summary.ContainsKey("minimum") || !summary.ContainsKey("maximum"))
                throw new ArgumentException("summary stats must contains minimum and maximum fields");
        }

        /// <summary>
        /// Initialize a Summary Range Object with a minimum and a maximum value
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public StacSummaryRangeObject(T min, T max) : base(new JObject())
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Minimum of the range
        /// </summary>
        /// <returns>Minimum of the range</returns>
        /// <value>
        /// <placeholder>Minimum of the range</placeholder>
        /// </value>
        public T Min { get => this.summary["minimum"].Value<T>(); set => this.summary["minimum"] = new JValue(value); }

        /// <summary>
        /// Maximum of the range
        /// </summary>
        /// <returns>Maximum of the range</returns>
        /// <value>
        /// <placeholder>Maximum of the range</placeholder>
        /// </value>
        public T Max { get => this.summary["maximum"].Value<T>(); set => this.summary["maximum"] = new JValue(value); }

        /// <inheritdoc/>
        public override IEnumerable<object> Enumerate()
        {
            return new object[2] { this.Min, this.Max };
        }
    }
}
