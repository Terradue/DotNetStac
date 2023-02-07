// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: AlternateStacExtension.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Stac.Extensions.Storage;

namespace Stac.Extensions.Alternate
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/alternate">Alternate extension</seealso>
    /// </summary>
    public class AlternateStacExtension : StacPropertiesContainerExtension, IStacAssetExtension, IStacExtension
    {
        // Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/alternate-assets/v1.1.0/schema.json";

        private const string AlternateField = "alternate";

        private static IDictionary<string, Type> assetFields;

        internal AlternateStacExtension(StacAsset stacAsset) : base(JsonSchemaUrl, stacAsset)
        {
            assetFields = new Dictionary<string, Type>();
            assetFields.Add(AlternateField, typeof(AlternateAssetObject[]));
        }

        /// <summary>
        /// Gets or sets a dictionary of alternate location information for an asset.
        /// </summary>
        /// <value>
        /// <placeholder>A dictionary of alternate location information for an asset.</placeholder>
        /// </value>
        public IDictionary<string, AlternateAssetObject> AlternateAssets
        {
            get { return this.StacPropertiesContainer.GetProperty<Dictionary<string, AlternateAssetObject>>(AlternateField); }
            set
            {
                if (value == null || value.Count() == 0)
                    this.StacPropertiesContainer.RemoveProperty(AlternateField);
                else
                {
                    this.StacPropertiesContainer.SetProperty(AlternateField, value);
                    this.DeclareStacExtension();
                }
            }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// <placeholder>Potential fields and their types</placeholder>
        /// </value>
        public override IDictionary<string, Type> ItemFields => assetFields;

        /// <inheritdoc/>
        public StacAsset StacAsset => this.StacPropertiesContainer as StacAsset;

        /// <inheritdoc/>
        public override IDictionary<string, ISummaryFunction> GetSummaryFunctions()
        {
            Dictionary<string, ISummaryFunction> summaryFunctions = new Dictionary<string, ISummaryFunction>();
            return summaryFunctions;
        }

        public AlternateAssetObject AddAlternate(string key, Uri uri, string title = null, string description = null)
        {
            AlternateAssetObject alternateAssetObject = new AlternateAssetObject(uri.ToString(), this.StacAsset.ParentStacObject, title, description);
            var alternateAssets = this.AlternateAssets ?? new Dictionary<string, AlternateAssetObject>();
            alternateAssets.Add(key, alternateAssetObject);
            this.AlternateAssets = alternateAssets;
            return alternateAssetObject;
        }

    }

    /// <summary>
    /// Extension methods for accessing Alternate extension
    /// </summary>
    public static class AlternateStacExtensionExtensions
    {

        /// <summary>
        /// Initilize a AlternateStacExtension class from a STAC asset
        /// </summary>
        public static AlternateStacExtension AlternateExtension(this StacAsset stacAsset)
        {
            return new AlternateStacExtension(stacAsset);
        }

        /// <summary>
        /// Initilize a AlternateStacExtension class from an alternate asset
        /// </summary>
        public static StorageStacExtension StorageExtension(this AlternateAssetObject alternateAssetObject)
        {
            return new StorageStacExtension(alternateAssetObject);
        }
    }
}
