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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/processing/v1.0.0/schema.json";
        public const string LineageField = "processing:lineage";
        public const string LevelField = "processing:level";
        public const string FacilityField = "processing:facility";
        public const string SoftwareField = "processing:software";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        public string Lineage
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(LineageField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(LineageField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the name commonly used to refer to the processing level to make it easier to search for product level across collections or items.
        /// </summary>
        public string Level
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(LevelField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(LevelField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets or sets the name of the facility that produced the data.
        /// </summary>
        public string Facility
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<string>(FacilityField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(FacilityField, value);
                this.DeclareStacExtension();
            }
        }

        /// <summary>
        /// Gets a dictionary with name/version for key/value describing one or more softwares that produced the data.
        /// </summary>
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
        public override IDictionary<string, Type> ItemFields => this._itemFields;

        private void UpdateSoftwareField(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.StacPropertiesContainer.SetProperty(SoftwareField, new Dictionary<string, string>(sender as IDictionary<string, string>));
            this.DeclareStacExtension();
        }
    }
}
