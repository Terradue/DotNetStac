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
        private readonly JToken _summary;

        /// <summary>
        /// Initializes a new instance of the <see cref="StacSummaryItem"/> class.
        /// </summary>
        /// <param name="summary">The summary.</param>
        protected StacSummaryItem(JToken summary)
        {
            this._summary = summary;
        }

        /// <summary>
        /// Gets jToken transformer
        /// </summary>
        /// <value>
        /// JToken transformer
        /// </value>
        public JToken AsJToken => this._summary;

        /// <summary>
        /// accessor of fields in the object
        /// </summary>
        /// <param name="key">key of the field</param>
        public JToken this[object key]
        {
            get
            {
                return this._summary[key];
            }
        }

        /// <inheritdoc/>
        public abstract IEnumerable<object> Enumerate();

        /// <summary>
        /// Get Enumerator of object children
        /// </summary>
        /// <returns>Enumerator of object children</returns>
        public IEnumerator GetEnumerator()
        {
            return this._summary.Children().GetEnumerator();
        }

        IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
        {
            return this._summary.Children().GetEnumerator();
        }
    }
}
