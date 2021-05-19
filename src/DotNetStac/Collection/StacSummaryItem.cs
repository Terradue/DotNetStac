using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    /// <summary>
    /// Base abstract class for a summary item
    /// </summary>
    public abstract class StacSummaryItem : IStacSummaryItem
    {
        /// <summary>
        /// Json Object
        /// </summary>
        protected readonly JToken summary;

        /// <summary>
        /// Initialize a new summary item with a JSON object
        /// </summary>
        /// <param name="summary"></param>
        protected StacSummaryItem(JToken summary)
        {
            this.summary = summary;
        }

        /// <summary>
        /// accessor of fields in the object
        /// </summary>
        /// <value></value>
        public JToken this[object key]
        {
            get
            {
                return summary[key];
            }
        }

        /// <summary>
        /// JToken transformer
        /// </summary>
        public JToken AsJToken => summary;

        /// <summary>
        /// Get Enumerator of object children
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return summary.Children().GetEnumerator();
        }

        IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
        {
            return summary.Children().GetEnumerator();
        }

    }
}