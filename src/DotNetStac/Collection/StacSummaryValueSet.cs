// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSummaryValueSet.cs

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
#pragma warning disable SA1649 // File name should match first type name

    /// <summary>
    /// Class representing a summary value set
    /// </summary>
    /// <typeparam name="T">Type of the value that is summarized</typeparam>
    public class StacSummaryValueSet<T> : StacSummaryItem, IEnumerable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StacSummaryValueSet{T}"/> class.
        /// </summary>
        public StacSummaryValueSet()
            : base(new JArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacSummaryValueSet{T}"/> class.
        /// Initialize a Summary Value Set with a JSON array
        /// </summary>
        /// <param name="summarySet">JSON Array</param>
        public StacSummaryValueSet(JArray summarySet)
            : base(summarySet)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StacSummaryValueSet{T}"/> class.
        /// Initialize a Summary Value Set with a set of values
        /// </summary>
        /// <param name="summarySet">set of values</param>
        public StacSummaryValueSet(IEnumerable<T> summarySet)
            : base(new JArray(summarySet))
        {
        }

        /// <summary>
        /// Gets summary Value Set total of items
        /// </summary>
        /// <value>
        /// Summary Value Set total of items
        /// </value>

        /// <value>
        /// Summary Value Set total of items
        /// </value>
        public int Count => this.AsJToken.Count();

        /// <summary>
        /// Gets get the Summary Value Set as an enumerable
        /// </summary>
        /// <value>
        /// Get the Summary Value Set as an enumerable
        /// </value>
        public IEnumerable<T> SummarySet { get => this.AsJToken.ToObject<List<T>>(); }

        /// <summary>
        /// Add a value item in the Summary Value Set
        /// </summary>
        /// <param name="item">value item</param>
        public void Add(T item)
        {
            ((JArray)this.AsJToken).Add(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.SummarySet.GetEnumerator();
        }

        /// <inheritdoc/>
        public override IEnumerable<object> Enumerate()
        {
            return this;
        }
    }

#pragma warning restore SA1649 // File name should match first type name
}
