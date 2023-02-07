// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ProcessingStacExtension.cs

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Stac.Model;

namespace Stac.Extensions.Processing
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/processing">Processing extension</seealso>
    /// </summary>
    public class ProcessingStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        // Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";
        private const string LineageField = "processing:lineage";
        private const string LevelField = "processing:level";
        private const string FacilityField = "processing:facility";
        private const string SoftwareField = "processing:software";
        private readonly Dictionary<string, Type> _itemFields;

        internal ProcessingStacExtension(StacItem stacItem)
            : base(JsonSchemaUrl, stacItem)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(LineageField, typeof(string));
            this._itemFields.Add(LevelField, typeof(string));
            this._itemFields.Add(FacilityField, typeof(string));
            this._itemFields.Add(SoftwareField, typeof(IDictionary<string, string>));
        }

        /// <summary>
        /// Gets or sets lineage Information provided as free text information about the how observations were processed or models that were used to create the resource being described NASA ISO.
        /// </summary>
        /// <value>
        /// Lineage Information provided as free text information about the how observations were processed or models that were used to create the resource being described NASA ISO.
        /// </value>
        public string Lineage
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(LineageField); }
            set { this.StacPropertiesContainer.SetProperty(LineageField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the name commonly used to refer to the processing level to make it easier to search for product level across collections or items.
        /// </summary>
        /// <value>
        /// The name commonly used to refer to the processing level to make it easier to search for product level across collections or items.
        /// </value>
        public string Level
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(LevelField); }
            set { this.StacPropertiesContainer.SetProperty(LevelField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the name of the facility that produced the data. 
        /// </summary>
        /// <value>
        /// The name of the facility that produced the data. 
        /// </value>
        public string Facility
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(FacilityField); }
            set { this.StacPropertiesContainer.SetProperty(FacilityField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets a dictionary with name/version for key/value describing one or more softwares that produced the data.
        /// </summary>
        /// <value>
        /// A dictionary with name/version for key/value describing one or more softwares that produced the data.
        /// </value>
        public IDictionary<string, string> Software
        {
            get
            {
                Dictionary<string, string> existingSoftware = this.StacPropertiesContainer.GetProperty<Dictionary<string, string>>(SoftwareField);
                ObservableDictionary<string, string> software = null;
                if (existingSoftware == null)
                {
                    software = new ObservableDictionary<string, string>();
                }
                else
                {
                    software = new ObservableDictionary<string, string>(existingSoftware);
                }

                software.CollectionChanged += this.UpdateSoftwareField;
                return software;
            }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// Potential fields and their types
        /// </value>
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        private void UpdateSoftwareField(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.StacPropertiesContainer.SetProperty(SoftwareField, new Dictionary<string, string>(sender as IDictionary<string, string>));
            this.DeclareStacExtension();
        }
    }

    /// <summary>
    /// Extension methods for accessing Processing extension
    /// </summary>
    public static class ProcessingStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a EoStacExtension class from a STAC item
        /// </summary>
        public static ProcessingStacExtension ProcessingExtension(this StacItem stacItem)
        {
            return new ProcessingStacExtension(stacItem);
        }

        /// <summary>
        /// Initialize the major fields of processing extensions
        /// </summary>
        public static void Init(
            this ProcessingStacExtension processingStacExtension,
            string lineage,
            string level,
            string facility = null)
        {
            processingStacExtension.Lineage = lineage;
            processingStacExtension.Level = level;
            processingStacExtension.Facility = facility;
        }
    }
}
