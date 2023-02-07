// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: IStacSummaryItem.cs

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
        /// Gets the summary item as a JToken
        /// </summary>
        /// <value>
        /// The summary item as a JToken
        /// </value>
        JToken AsJToken { get; }

        /// <summary>
        /// Gets or sets the summary value with the specified fields (for objects only)
        /// </summary>
        /// <value>
        /// The <see cref="JToken"/>.
        /// </value>
        /// <param name="key">The key.</param>
        JToken this[object key] { get; }

        /// <summary>
        /// Gets the summary item as a JToken
        /// </summary>
        /// <value>
        /// The summary item as a JToken
        /// </value>
        /// <returns>The summary item as a JToken</returns>
        IEnumerable<object> Enumerate();
    }
}
