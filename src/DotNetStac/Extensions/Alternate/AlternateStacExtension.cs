// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: AlternateStacExtension.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace Stac.Extensions.Alternate
{
    /// <summary>
    /// Helper class to access the fields defined by the <seealso href="https://github.com/stac-extensions/alternate">Alternate extension</seealso>
    /// </summary>
    public class AlternateStacExtension : StacPropertiesContainerExtension, IStacAssetExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/alternate-assets/v1.1.0/schema.json";

        private const string AlternateField = "alternate";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private static IDictionary<string, Type> assetFields;

        internal AlternateStacExtension(StacAsset stacAsset)
            : base(JsonSchemaUrl, stacAsset)
        {
            assetFields = new Dictionary<string, Type>();
            assetFields.Add(AlternateField, typeof(AlternateAssetObject[]));
        }

        /// <summary>
        /// Gets or sets a dictionary of alternate location information for an asset.
        /// </summary>
        /// <value>
        /// A dictionary of alternate location information for an asset.
        /// </value>
        public IDictionary<string, AlternateAssetObject> AlternateAssets
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<Dictionary<string, AlternateAssetObject>>(AlternateField);
            }

            set
            {
                if (value == null || value.Count() == 0)
                {
                    this.StacPropertiesContainer.RemoveProperty(AlternateField);
                }
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
        /// Potential fields and their types
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

        /// <summary>
        /// Adds an alternate asset to the AlternateAssets dictionary
        /// </summary>
        /// <param name="key">The key of the alternate asset</param>
        /// <param name="uri">The uri of the alternate asset</param>
        /// <param name="title">The title of the alternate asset</param>
        /// <param name="description">The description of the alternate asset</param>
        /// <returns>The alternate asset object</returns>
        public AlternateAssetObject AddAlternate(string key, Uri uri, string title = null, string description = null)
        {
            AlternateAssetObject alternateAssetObject = new AlternateAssetObject(uri.ToString(), this.StacAsset.ParentStacObject, title, description);
            var alternateAssets = this.AlternateAssets ?? new Dictionary<string, AlternateAssetObject>();
            alternateAssets.Add(key, alternateAssetObject);
            this.AlternateAssets = alternateAssets;
            return alternateAssetObject;
        }
    }
}
