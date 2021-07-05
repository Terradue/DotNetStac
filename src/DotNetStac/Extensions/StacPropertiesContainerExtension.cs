using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Collection;
using Stac.Exceptions;

public delegate IStacSummaryItem CreateSummary<T>(IEnumerable<T> arg);

namespace Stac.Extensions
{
    /// <summary>
    /// Abstract class for a generic extension based on a STAC object that
    /// implements <see cref="IStacPropertiesContainer" />
    /// This is a base for building an extension based on new fields for an
    /// object having a properties
    /// </summary>
    public abstract class StacPropertiesContainerExtension : IStacExtension
    {
        private readonly string identifier;

        /// <summary>
        /// Initialize a new class for a STAC object implementing <see cref="IStacPropertiesContainer" />.
        /// </summary>
        /// <param name="identifier">Identifier of the extension</param>
        /// <param name="stacPropertiesContainer">STAC object</param>
        protected StacPropertiesContainerExtension(string identifier, IStacPropertiesContainer stacPropertiesContainer)
        {
            this.identifier = identifier;
            StacPropertiesContainer = stacPropertiesContainer;
        }

        /// <summary>
        /// Identifier of the extension
        /// </summary>
        public virtual string Identifier => identifier;

        /// <summary>
        /// Stac Object extended by the extension
        /// </summary>
        public IStacPropertiesContainer StacPropertiesContainer { get; private set; }

        /// <summary>
        /// Get the potential fields of the extensions and their type
        /// </summary>
        public abstract IDictionary<string, Type> ItemFields { get; }

        /// <summary>
        /// Indicate if the extension is already declared
        /// </summary>
        public bool IsDeclared => StacPropertiesContainer.GetDeclaredExtensions().Any(e => e.Identifier == Identifier);

        /// <summary>
        /// Get he potential summary functions for each field that can be summarized
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();

            foreach (var itemField in ItemFields)
            {
                if (itemField.Value == typeof(bool))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<bool>(this, itemField.Key, CreateSummaryValueSet<bool>));
                else if (itemField.Value == typeof(short))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<short>(this, itemField.Key, CreateRangeSummaryObject<short>));
                else if (itemField.Value == typeof(int))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<int>(this, itemField.Key, CreateRangeSummaryObject<int>));
                else if (itemField.Value == typeof(long))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<long>(this, itemField.Key, CreateRangeSummaryObject<long>));
                else if (itemField.Value == typeof(float))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<float>(this, itemField.Key, CreateRangeSummaryObject<float>));
                else if (itemField.Value == typeof(double))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<double>(this, itemField.Key, CreateRangeSummaryObject<double>));
                else if (itemField.Value == typeof(DateTime))
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<DateTime>(this, itemField.Key, CreateRangeSummaryObject<DateTime>));
                else
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<string>(this, itemField.Key, CreateSummaryValueSet<string>));
            }
            return summaryFunctions;
        }

        /// <summary>
        /// Generic method to summarize in a range any ordinal object
        /// </summary>
        /// <param name="arg">ordinal object</param>
        public static IStacSummaryItem CreateRangeSummaryObject<T>(IEnumerable<T> arg)
        {
            return new StacSummaryRangeObject<object>(arg.Min(), arg.Max());
        }

        /// <summary>
        /// Generic method to summarize in a value set an array of object
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static StacSummaryValueSet<T> CreateSummaryValueSet<T>(IEnumerable<T> arg)
        {
            return new StacSummaryValueSet<T>(arg.Distinct());
        }

        /// <summary>
        /// Generic method to summarize in a value set an array of array
        /// </summary>
        /// <param name="arg">an array</param>
        /// <returns></returns>
        // internal static IStacSummaryItem CreateSummaryValueSetFromArrays<T>(IEnumerable<T> arg)
        // {
        //     return new StacSummaryValueSet<T>(arg.SelectMany(a => a as IEnumerable<T>).Distinct());
        // }

        /// <summary>
        /// Declares the extension in the STAC object
        /// </summary>
        protected void DeclareStacExtension()
        {
            if (StacPropertiesContainer.StacObjectContainer == null) return;
            if (!StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(Identifier))
                StacPropertiesContainer.StacObjectContainer.StacExtensions.Add(Identifier);
        }

        /// <summary>
        /// Remove the extension in the STAC object
        /// </summary>
        protected void RemoveStacExtension()
        {
            if (StacPropertiesContainer.StacObjectContainer == null) return;
            if (StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(Identifier))
                StacPropertiesContainer.StacObjectContainer.StacExtensions.Remove(Identifier);
        }

        public void SetProperty(string key, object value)
        {
            this.SetProperty(key, value);
            DeclareStacExtension();
        }

        public void RemoveProperty(string key)
        {
            this.RemoveProperty(key);
            if (!StacPropertiesContainer.Properties.Any(p => ItemFields.ContainsKey(p.Key)))
                RemoveStacExtension();
        }
    }
}
