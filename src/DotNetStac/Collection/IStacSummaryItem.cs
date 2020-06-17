using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    public interface IStacSummaryItem : IEnumerable<JToken>
    {
        SummaryItemType SummaryType { get; }

        JToken this[object key] { get; }

        JToken AsJToken { get; }

    }
}