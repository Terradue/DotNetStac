using System;
using System.Collections.Generic;
using Stac.Collection;

namespace Stac.Extensions
{
    public interface IStacExtension
    {
        string Identifier { get; }

        IDictionary<string, Func<IEnumerable<object>, IStacSummaryItem>> GetSummaryFunctions();
    }
}