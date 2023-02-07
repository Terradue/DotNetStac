// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StorageStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.Storage
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/storage">Storage extension</seealso>
    /// </summary>
    public class StorageStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        // Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/storage/v1.0.0/schema.json";
        public const string PlatformField = "storage:platform";
        public const string RegionField = "storage:region";
        public const string RequesterPaysField = "storage:requester_pays";
        public const string TierField = "storage:tier";
        private readonly Dictionary<string, Type> _itemFields;

        internal StorageStacExtension(IStacPropertiesContainer stacObject)
            : base(JsonSchemaUrl, stacObject)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(PlatformField, typeof(string));
            this._itemFields.Add(RegionField, typeof(string));
            this._itemFields.Add(RequesterPaysField, typeof(bool));
            this._itemFields.Add(TierField, typeof(string));
        }

        /// <summary>
        /// Gets or sets the cloud provider where data is stored
        /// </summary>
        /// <value>
        /// The cloud provider where data is stored
        /// </value>
        public string Platform
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(PlatformField); }
            set { this.StacPropertiesContainer.SetProperty(PlatformField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the region where the data is stored. Relevant to speed of access and inter region egress costs (as defined by PaaS provider)
        /// </summary>
        /// <value>
        /// The region where the data is stored. Relevant to speed of access and inter region egress costs (as defined by PaaS provider)
        /// </value>
        public string Region
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(RegionField); }
            set { this.StacPropertiesContainer.SetProperty(RegionField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets is the data requester pays or is it data manager/cloud provider pays. Defaults to false
        /// </summary>
        /// <value>
        /// Is the data requester pays or is it data manager/cloud provider pays. Defaults to false
        /// </value>
        public bool? RequesterPays
        {
            get { return this.StacPropertiesContainer.GetProperty<bool?>(RequesterPaysField); }
            set { this.StacPropertiesContainer.SetProperty(RequesterPaysField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets or sets the title for the tier type (as defined by PaaS provider)
        /// </summary>
        /// <value>
        /// The title for the tier type (as defined by PaaS provider)
        /// </value>
        public string Tier
        {
            get { return this.StacPropertiesContainer.GetProperty<string>(TierField); }
            set { this.StacPropertiesContainer.SetProperty(TierField, value); this.DeclareStacExtension(); }
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
    }

    /// <summary>
    /// Extension methods for accessing Storage extension
    /// </summary>
    public static class StorageStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a StorageStacExtension class from a STAC Asset
        /// </summary>
        public static StorageStacExtension StorageExtension(this StacAsset stacAsset)
        {
            return new StorageStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a StorageStacExtension class from a STAC Item
        /// </summary>
        public static StorageStacExtension StorageExtension(this StacItem stacItem)
        {
            return new StorageStacExtension(stacItem);
        }
    }
}
