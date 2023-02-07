// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ISummaryFunction.cs

using System.Collections.Generic;
using Stac.Collection;

namespace Stac.Extensions
{
    public interface ISummaryFunction
    {
        IStacExtension Extension { get; }

        string PropertyName { get; }

        IStacSummaryItem Summarize(IEnumerable<object> items);
    }
}
