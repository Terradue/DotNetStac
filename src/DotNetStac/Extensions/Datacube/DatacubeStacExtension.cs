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
        /// Extensions identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/datacube/v2.1.0/schema.json";

        private readonly IDictionary<string, Type> itemFields;
        private const string DimensionField = "cube:dimensions";
        private const string VariableField = "cube:variables";

        private DatacubeStacExtension(IStacPropertiesContainer stacPropertiesContainer) : base(JsonSchemaUrl, stacPropertiesContainer)
        {
            itemFields = new Dictionary<string, Type>();
            itemFields.Add(DimensionField, typeof(IDictionary<string, DatacubeDimension>));
            itemFields.Add(VariableField, typeof(IDictionary<string, DatacubeVariable>));
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
        /// A dictionary of available dimensions where each object is a DatacubeDimension Object.
        /// </summary>
        /// <value>
        /// <placeholder>A dictionary of available dimensions where each object is a DatacubeDimension Object.</placeholder>
        /// </value>
        public IDictionary<string, DatacubeDimension> Dimensions
        {
            get
            {
                Dictionary<string, DatacubeDimension> existingDimensions = StacPropertiesContainer.GetProperty<Dictionary<string, DatacubeDimension>>(DimensionField);
                ObservableDictionary<string, DatacubeDimension> dimensions = null;
                if (existingDimensions == null)
                    dimensions = new ObservableDictionary<string, DatacubeDimension>();
                else
                    dimensions = new ObservableDictionary<string, DatacubeDimension>(existingDimensions);
                dimensions.CollectionChanged += UpdateDimensionField;
                return dimensions;
            }
        }

        private void UpdateDimensionField(object sender, NotifyCollectionChangedEventArgs e)
        {
            StacPropertiesContainer.SetProperty(DimensionField, new Dictionary<string, DatacubeDimension>(sender as IDictionary<string, DatacubeDimension>));
            DeclareStacExtension();
        }

        /// <summary>
        /// A dictionary of available variables where each object is a DatacubeVariable Object.
        /// </summary>
        /// <value>
        /// <placeholder>A dictionary of available variables where each object is a DatacubeVariable Object.</placeholder>
        /// </value>
        public IDictionary<string, DatacubeVariable> Variables
        {
            get
            {
                Dictionary<string, DatacubeVariable> existingVariables = StacPropertiesContainer.GetProperty<Dictionary<string, DatacubeVariable>>(VariableField);
                ObservableDictionary<string, DatacubeVariable> variables = null;
                if (existingVariables == null)
                    variables = new ObservableDictionary<string, DatacubeVariable>();
                else
                    variables = new ObservableDictionary<string, DatacubeVariable>(existingVariables);
                variables.CollectionChanged += UpdateVariableField;
                return variables;
            }
        }

        private void UpdateVariableField(object sender, NotifyCollectionChangedEventArgs e)
        {
            StacPropertiesContainer.SetProperty(VariableField, new Dictionary<string, DatacubeVariable>(sender as IDictionary<string, DatacubeVariable>));
            DeclareStacExtension();
        }

        /// <summary>
        /// Potential fields and their types
        /// </summary>
        /// <value>
        /// <placeholder>Potential fields and their types</placeholder>
        /// </value>
        public override IDictionary<string, Type> ItemFields => itemFields;

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
