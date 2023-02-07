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
        // Extensions identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/datacube/v2.1.0/schema.json";

        private readonly IDictionary<string, Type> itemFields;
        private const string DimensionField = "cube:dimensions";
        private const string VariableField = "cube:variables";

        private DatacubeStacExtension(IStacPropertiesContainer stacPropertiesContainer) : base(JsonSchemaUrl, stacPropertiesContainer)
        {
            this.itemFields = new Dictionary<string, Type>();
            this.itemFields.Add(DimensionField, typeof(IDictionary<string, DatacubeDimension>));
            this.itemFields.Add(VariableField, typeof(IDictionary<string, DatacubeVariable>));
        }

        internal DatacubeStacExtension(StacCollection stacCollection) : this((IStacPropertiesContainer)stacCollection)
        {
        }

        internal DatacubeStacExtension(StacAsset stacAsset) : this((IStacPropertiesContainer)stacAsset)
        {
        }

        internal DatacubeStacExtension(StacItem stacItem) : this((IStacPropertiesContainer)stacItem)
        {
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

        private void UpdateDimensionField(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.StacPropertiesContainer.SetProperty(DimensionField, new Dictionary<string, DatacubeDimension>(sender as IDictionary<string, DatacubeDimension>));
            this.DeclareStacExtension();
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

        private void UpdateVariableField(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.StacPropertiesContainer.SetProperty(VariableField, new Dictionary<string, DatacubeVariable>(sender as IDictionary<string, DatacubeVariable>));
            this.DeclareStacExtension();
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// Potential fields and their types
        /// </value>
        public override IDictionary<string, Type> ItemFields => this.itemFields;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }
    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class DatacubeStacExtensionExtensions
    {

        /// <summary>
        /// Initilize a DatacubeStacExtension class from a STAC asset
        /// </summary>
        public static DatacubeStacExtension DatacubeStacExtension(this StacAsset stacAsset)
        {
            return new DatacubeStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a DatacubeStacExtension class from a STAC item
        /// </summary>
        public static DatacubeStacExtension DatacubeStacExtension(this StacItem stacItem)
        {
            return new DatacubeStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a DatacubeStacExtension class from a STAC collection
        /// </summary>
        public static DatacubeStacExtension DatacubeStacExtension(this StacCollection stacCollection)
        {
            return new DatacubeStacExtension(stacCollection);
        }
    }
}
