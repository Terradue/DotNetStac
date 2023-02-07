// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SummaryFunction.cs

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac.Collection;

namespace Stac.Extensions
{
    public class SummaryFunction<T> : ISummaryFunction
    {
        private readonly CreateSummary<T> _summaryFunction;

        public SummaryFunction(IStacExtension extension, string propertyName, CreateSummary<T> summaryFunction)
        {
            this.Extension = extension;
            this.PropertyName = propertyName;
            this._summaryFunction = summaryFunction;
        }

        /// <inheritdoc/>
        public IStacExtension Extension { get; }

        /// <inheritdoc/>
        public string PropertyName { get; }

        /// <inheritdoc/>
        public IStacSummaryItem Summarize(IEnumerable<object> items)
        {
            return this._summaryFunction(items.SelectMany(i =>
            {
                try
                {
                    if (i is JArray)
                    {
                        return (i as IEnumerable).OfType<object>().Select(i2 => StacAccessorsHelpers.ChangeType<T>(i2));
                    }

                    return new T[] { StacAccessorsHelpers.ChangeType<T>(i) };
                }
                catch
                {
                    return new T[0];
                }
            }));
        }
    }
}
