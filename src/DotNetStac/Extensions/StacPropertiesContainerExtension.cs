using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Collection;
using Stac.Exceptions;

public delegate IStacSummaryItem CreateSummary(IEnumerable<object> arg);

namespace Stac.Extensions
{
    public abstract class StacPropertiesContainerExtension : IStacExtension
    {
        private readonly string identifier;

        public StacPropertiesContainerExtension(string identifier, IStacPropertiesContainer stacPropertiesContainer)
        {
            this.identifier = identifier;
            StacPropertiesContainer = stacPropertiesContainer;
        }

        public virtual string Identifier => identifier;

        public IStacPropertiesContainer StacPropertiesContainer { get; private set; }

        public abstract IDictionary<string, Type> ItemFields { get; }

        public virtual IDictionary<string, SummaryFunction> GetSummaryFunctions()
        {

            Dictionary<string, SummaryFunction> summaryFunctions = new Dictionary<string, SummaryFunction>();

            foreach (var itemField in ItemFields)
            {
                CreateSummary summaryFunction = CreateSummaryValueSet;
                if (itemField.Value == typeof(bool) || itemField.Value == typeof(short) || itemField.Value == typeof(int) || itemField.Value == typeof(long) ||
                        itemField.Value == typeof(float) || itemField.Value == typeof(double) || itemField.Value == typeof(DateTime))
                    summaryFunction = CreateSummaryStatsObject;
                summaryFunctions.Add(itemField.Key, new SummaryFunction(this, itemField.Key, summaryFunction));
            }
            return summaryFunctions;
        }

        public static IStacSummaryItem CreateSummaryStatsObject(IEnumerable<object> arg)
        {
            return new StacSummaryRangeObject<object>(arg.Min(), arg.Max());
        }

        public static StacSummaryValueSet<object> CreateSummaryValueSet(IEnumerable<object> arg)
        {
            return new StacSummaryValueSet<object>(arg.Distinct());
        }

        protected void DeclareStacExtension()
        {
            if (!StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(Identifier))
                StacPropertiesContainer.StacObjectContainer.StacExtensions.Add(Identifier);
        }

        internal static IStacSummaryItem CreateSummaryValueSetFromArrays(IEnumerable<object> arg)
        {
            return new StacSummaryValueSet<object>(arg.SelectMany(a => a as IEnumerable<object>).Distinct());
        }
    }
}