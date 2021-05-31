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
        public virtual IDictionary<string, SummaryFunction> GetSummaryFunctions()
        {

            Dictionary<string, SummaryFunction> summaryFunctions = new Dictionary<string, SummaryFunction>();

            foreach (var itemField in ItemFields)
            {
                CreateSummary summaryFunction = CreateSummaryValueSet;
                if (itemField.Value == typeof(bool) || itemField.Value == typeof(short) || itemField.Value == typeof(int) || itemField.Value == typeof(long) ||
                        itemField.Value == typeof(float) || itemField.Value == typeof(double) || itemField.Value == typeof(DateTime))
                    summaryFunction = CreateRangeSummaryObject;
                summaryFunctions.Add(itemField.Key, new SummaryFunction(this, itemField.Key, summaryFunction));
            }
            return summaryFunctions;
        }

        /// <summary>
        /// Generic method to summarize in a range any ordinal object
        /// </summary>
        /// <param name="arg">ordinal object</param>
        public static IStacSummaryItem CreateRangeSummaryObject(IEnumerable<object> arg)
        {
            return new StacSummaryRangeObject<object>(arg.Min(), arg.Max());
        }

        /// <summary>
        /// Generic method to summarize in a value set an array of object
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static StacSummaryValueSet<object> CreateSummaryValueSet(IEnumerable<object> arg)
        {
            return new StacSummaryValueSet<object>(arg.Distinct());
        }

        /// <summary>
        /// Generic method to summarize in a value set an array of array
        /// </summary>
        /// <param name="arg">an array</param>
        /// <returns></returns>
        internal static IStacSummaryItem CreateSummaryValueSetFromArrays(IEnumerable<object> arg)
        {
            return new StacSummaryValueSet<object>(arg.SelectMany(a => a as IEnumerable<object>).Distinct());
        }

        /// <summary>
        /// Declares the extension in the STAC object
        /// </summary>
        protected void DeclareStacExtension()
        {
            if (StacPropertiesContainer.StacObjectContainer == null) return;
            if (!StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(Identifier))
                StacPropertiesContainer.StacObjectContainer.StacExtensions.Add(Identifier);
        }


    }
}
