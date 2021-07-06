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
        /// <exception cref="System.ArgumentException">Thrown when neither "minimum" nor "maximum" fields are present in the range object.</exception>
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
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Minimum of the range
        /// </summary>
        /// <returns>Minimum of the range</returns>
        public T Min { get => summary["minimum"].Value<T>(); set => summary["minimum"] = new JValue(value); }

        /// <summary>
        /// Maximum of the range
        /// </summary>
        /// <returns>Maximum of the range</returns>
        public T Max { get => summary["maximum"].Value<T>(); set => summary["maximum"] = new JValue(value); }

        public override IEnumerable<object> Enumerate()
        {
            return new object[2] { Min, Max };
        }
    }
}
