// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSummaryItem.cs

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

        public JToken this[object key]
        {
            get
            {
                return this.summary[key];
            }
        }

        /// <summary>
        /// Gets jToken transformer
        /// </summary>
        /// <value>
        /// <placeholder>JToken transformer</placeholder>
        /// </value>
        public JToken AsJToken => this.summary;

        /// <inheritdoc/>
        public abstract IEnumerable<object> Enumerate();

        /// <summary>
        /// Get Enumerator of object children
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return this.summary.Children().GetEnumerator();
        }

        IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
        {
            return this.summary.Children().GetEnumerator();
        }

    }
}
