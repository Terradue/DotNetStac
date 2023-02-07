// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacPropertiesContainerExtension.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Stac.Collection;

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
            this.StacPropertiesContainer = stacPropertiesContainer;
        }

        /// <summary>
        /// Gets identifier of the extension
        /// </summary>
        /// <value>
        /// Identifier of the extension
        /// </value>
        public virtual string Identifier => this.identifier;

        /// <summary>
        /// Gets stac Object extended by the extension
        /// </summary>
        /// <value>
        /// Stac Object extended by the extension
        /// </value>
        public IStacPropertiesContainer StacPropertiesContainer { get; private set; }

        /// <summary>
        /// Gets get the potential fields of the extensions and their type
        /// </summary>
        /// <value>
        /// Get the potential fields of the extensions and their type
        /// </value>
        public abstract IDictionary<string, Type> ItemFields { get; }

        /// <summary>
        /// Gets a value indicating whether indicate if the extension is already declared
        /// </summary>
        /// <value>
        /// Indicate if the extension is already declared
        /// </value>
        public bool IsDeclared => this.StacPropertiesContainer.GetDeclaredExtensions().Any(e => e.Identifier == this.Identifier);

        /// <summary>
        /// Get he potential summary functions for each field that can be summarized
        /// </summary>
        public virtual IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();

            foreach (var itemField in this.ItemFields)
            {
                if (itemField.Value == typeof(bool))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<bool>(this, itemField.Key, CreateSummaryValueSet));
                }
                else if (itemField.Value == typeof(short))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<short>(this, itemField.Key, CreateRangeSummaryObject));
                }
                else if (itemField.Value == typeof(int))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<int>(this, itemField.Key, CreateRangeSummaryObject));
                }
                else if (itemField.Value == typeof(long))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<long>(this, itemField.Key, CreateRangeSummaryObject));
                }
                else if (itemField.Value == typeof(float))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<float>(this, itemField.Key, CreateRangeSummaryObject));
                }
                else if (itemField.Value == typeof(double))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<double>(this, itemField.Key, CreateRangeSummaryObject));
                }
                else if (itemField.Value == typeof(DateTime))
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<DateTime>(this, itemField.Key, CreateRangeSummaryObject));
                }
                else
                {
                    summaryFunctions.Add(itemField.Key, new SummaryFunction<string>(this, itemField.Key, CreateSummaryValueSet));
                }
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
        public static StacSummaryValueSet<T> CreateSummaryValueSet<T>(IEnumerable<T> arg)
        {
            return new StacSummaryValueSet<T>(arg.Distinct());
        }

        /// <summary>
        /// Generic method to summarize in a value set an array of array
        /// </summary>
        /// <param name="arg">an array</param>

        // internal static IStacSummaryItem CreateSummaryValueSetFromArrays<T>(IEnumerable<T> arg)
        // {
        //     return new StacSummaryValueSet<T>(arg.SelectMany(a => a as IEnumerable<T>).Distinct());
        // }

        /// <summary>
        /// Declares the extension in the STAC object
        /// </summary>
        protected void DeclareStacExtension()
        {
            if (this.StacPropertiesContainer.StacObjectContainer == null)
            {
                return;
            }

            if (!this.StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(this.Identifier))
            {
                this.StacPropertiesContainer.StacObjectContainer.StacExtensions.Add(this.Identifier);
            }
        }

        /// <summary>
        /// Remove the extension in the STAC object
        /// </summary>
        protected void RemoveStacExtension()
        {
            if (this.StacPropertiesContainer.StacObjectContainer == null)
            {
                return;
            }

            if (this.StacPropertiesContainer.StacObjectContainer.StacExtensions.Contains(this.Identifier))
            {
                this.StacPropertiesContainer.StacObjectContainer.StacExtensions.Remove(this.Identifier);
            }
        }

        public void SetProperty(string key, object value)
        {
            this.SetProperty(key, value);
            this.DeclareStacExtension();
        }

        public void RemoveProperty(string key)
        {
            this.RemoveProperty(key);
            if (!this.StacPropertiesContainer.Properties.Any(p => this.ItemFields.ContainsKey(p.Key)))
            {
                this.RemoveStacExtension();
            }
        }
    }
}

public delegate IStacSummaryItem CreateSummary<T>(IEnumerable<T> arg);
