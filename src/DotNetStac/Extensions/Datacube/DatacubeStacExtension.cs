// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DatacubeStacExtension.cs

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Stac.Model;

namespace Stac.Extensions.Datacube
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/datacube">Datacube extension</seealso>
    /// </summary>
    public class DatacubeStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/datacube/v2.1.0/schema.json";

        private const string DimensionField = "cube:dimensions";
        private const string VariableField = "cube:variables";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private readonly IDictionary<string, Type> _itemFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeStacExtension"/> class.
        /// </summary>
        /// <param name="stacCollection">The stac collection.</param>
        internal DatacubeStacExtension(StacCollection stacCollection)
            : this((IStacPropertiesContainer)stacCollection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeStacExtension"/> class.
        /// </summary>
        /// <param name="stacAsset">The stac asset.</param>
        internal DatacubeStacExtension(StacAsset stacAsset)
            : this((IStacPropertiesContainer)stacAsset)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeStacExtension"/> class.
        /// </summary>
        /// <param name="stacItem">The stac item.</param>
        internal DatacubeStacExtension(StacItem stacItem)
            : this((IStacPropertiesContainer)stacItem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatacubeStacExtension"/> class.
        /// </summary>
        /// <param name="stacPropertiesContainer">The stac properties container.</param>
        private DatacubeStacExtension(IStacPropertiesContainer stacPropertiesContainer)
            : base(JsonSchemaUrl, stacPropertiesContainer)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(DimensionField, typeof(IDictionary<string, DatacubeDimension>));
            this._itemFields.Add(VariableField, typeof(IDictionary<string, DatacubeVariable>));
        }

        /// <summary>
        /// Gets a dictionary of available dimensions where each object is a DatacubeDimension Object.
        /// </summary>
        /// <value>
        /// A dictionary of available dimensions where each object is a DatacubeDimension Object.
        /// </value>
        public IDictionary<string, DatacubeDimension> Dimensions
        {
            get
            {
                Dictionary<string, DatacubeDimension> existingDimensions = this.StacPropertiesContainer.GetProperty<Dictionary<string, DatacubeDimension>>(DimensionField);
                ObservableDictionary<string, DatacubeDimension> dimensions = null;
                if (existingDimensions == null)
                {
                    dimensions = new ObservableDictionary<string, DatacubeDimension>();
                }
                else
                {
                    dimensions = new ObservableDictionary<string, DatacubeDimension>(existingDimensions);
                }

                dimensions.CollectionChanged += this.UpdateDimensionField;
                return dimensions;
            }
        }

        /// <summary>
        /// Gets a dictionary of available variables where each object is a DatacubeVariable Object.
        /// </summary>
        /// <value>
        /// A dictionary of available variables where each object is a DatacubeVariable Object.
        /// </value>
        public IDictionary<string, DatacubeVariable> Variables
        {
            get
            {
                Dictionary<string, DatacubeVariable> existingVariables = this.StacPropertiesContainer.GetProperty<Dictionary<string, DatacubeVariable>>(VariableField);
                ObservableDictionary<string, DatacubeVariable> variables = null;
                if (existingVariables == null)
                {
                    variables = new ObservableDictionary<string, DatacubeVariable>();
                }
                else
                {
                    variables = new ObservableDictionary<string, DatacubeVariable>(existingVariables);
                }

                variables.CollectionChanged += this.UpdateVariableField;
                return variables;
            }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// Potential fields and their types
        /// </value>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }

        private void UpdateDimensionField(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.StacPropertiesContainer.SetProperty(DimensionField, new Dictionary<string, DatacubeDimension>(sender as IDictionary<string, DatacubeDimension>));
            this.DeclareStacExtension();
        }

        private void UpdateVariableField(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.StacPropertiesContainer.SetProperty(VariableField, new Dictionary<string, DatacubeVariable>(sender as IDictionary<string, DatacubeVariable>));
            this.DeclareStacExtension();
        }
    }
}
