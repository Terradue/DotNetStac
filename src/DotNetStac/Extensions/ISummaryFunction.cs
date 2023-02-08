// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ISummaryFunction.cs

using System.Collections.Generic;
using Stac.Collection;

namespace Stac.Extensions
{
    /// <summary>
    /// Interface for Stac Extension summary functions
    /// </summary>
    public interface ISummaryFunction
    {
        /// <summary>
        /// Gets the Stac Extension associated to the summary function.
        /// </summary>
        IStacExtension Extension { get; }

        /// <summary>
        /// Gets the PropertyName of the summary function.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Summarize the items.
        /// </summary>
        /// <param name="items">The items to summarize.</param>
        /// <returns>The summary item.</returns>
        IStacSummaryItem Summarize(IEnumerable<object> items);
    }
}
