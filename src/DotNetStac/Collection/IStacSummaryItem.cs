using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    /// <summary>
    /// Provides the summary item interface for Collections
    /// </summary>
    public interface IStacSummaryItem : IEnumerable<JToken>
    {
        /// <summary>
        /// Gets or sets the summary value with the specified fields (for objects only)
        /// </summary>
        /// <value></value>
        JToken this[object key] { get; }

        /// <summary>
        /// Gets or sets the summary item as a JToken
        /// </summary>
        /// <value></value>
        JToken AsJToken { get; }

    }
}