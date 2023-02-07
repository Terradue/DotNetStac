﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacSummaryValueSet.cs

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    /// <summary>
    /// Class representing a summary value set
    /// </summary>
    /// <typeparam name="T">Type of the value that is summarized</typeparam>
    public class StacSummaryValueSet<T> : StacSummaryItem, IEnumerable<T>
    {
        /// <summary>
        /// Initialize an empty Summary Value Set
        /// </summary>
        public StacSummaryValueSet() : base(new JArray())
        {
        }

        /// <summary>
        /// Initialize a Summary Value Set with a JSON array
        /// </summary>
        /// <param name="summarySet">JSON Array</param>
        public StacSummaryValueSet(JArray summarySet) : base(summarySet)
        {
        }

        /// <summary>
        /// Initialize a Summary Value Set with a set of values
        /// </summary>
        /// <param name="summarySet">set of values</param>
        public StacSummaryValueSet(IEnumerable<T> summarySet) : base(new JArray(summarySet))
        {
        }

        /// <summary>
        /// Add a value item in the Summary Value Set
        /// </summary>
        /// <param name="item">value item</param>
        public void Add(T item)
        {
            ((JArray)summary).Add(item);
        }

        /// <summary>
        /// Summary Value Set total of items
        /// </summary>
        /// <returns></returns>
        /// <value>
        /// <placeholder>Summary Value Set total of items</placeholder>
        /// </value>
        public int Count => summary.Count();

        /// <summary>
        /// Get the Summary Value Set as an enumerable
        /// </summary>
        /// <value>
        /// <placeholder>Get the Summary Value Set as an enumerable</placeholder>
        /// </value>
        public IEnumerable<T> SummarySet { get => summary.ToObject<List<T>>(); }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return SummarySet.GetEnumerator();
        }

        /// <inheritdoc/>
        public override IEnumerable<object> Enumerate()
        {
            return this;
        }
    }
}
