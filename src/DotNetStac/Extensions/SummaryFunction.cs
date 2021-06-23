using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stac.Collection;

namespace Stac.Extensions
{
    public class SummaryFunction<T> : ISummaryFunction
    {
        public IStacExtension Extension { get; }

        public string PropertyName { get; }

        private CreateSummary<T> summaryFunction;

        public IStacSummaryItem Summarize(IEnumerable<object> items)
        {
            return summaryFunction(items.SelectMany(i =>
            {
                try
                {
                    if (i is JArray)
                        return (i as IEnumerable).OfType<object>().Select(i2 => StacAccessorsHelpers.ChangeType<T>(i2));

                    return new T[] { StacAccessorsHelpers.ChangeType<T>(i) };
                }
                catch
                {
                    return new T[0];
                }
            }));
        }

        public SummaryFunction(IStacExtension extension, string propertyName, CreateSummary<T> summaryFunction)
        {
            Extension = extension;
            PropertyName = propertyName;
            this.summaryFunction = summaryFunction;
        }
    }
}
