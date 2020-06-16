using System;
using System.Collections;
using System.Collections.Generic;

namespace Stac.Collection
{
    public interface IStacSummaryItem : IEnumerable
    {
        SummaryItemType SummaryType { get; }

        Type ValueType { get; }
    }
}