using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Stac.Collection
{
    public abstract class StacSummaryItem : IStacSummaryItem
    {
        protected readonly JToken summary;

        protected StacSummaryItem(JToken summary)
        {
            this.summary = summary;
        }

        public JToken this[object key]
        {
            get
            {
                return summary[key];
            }
        }

        public JToken AsJToken => summary;

        public IEnumerator GetEnumerator()
        {
            return summary.Children().GetEnumerator();
        }

        IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
        {
            return summary.Children().GetEnumerator();
        }

    }
}