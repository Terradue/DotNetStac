// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VirtualAssetsStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.VirtualAssets
{
    /// <summary>
    /// Helper class to access the fields deined by the <seealso href="https://github.com/stac-extensions/eo">EO extension</seealso>
    /// </summary>
    public class VirtualAssetsStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
        // Extension identifier and schema url
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/virtual-assets/v1.0.0/schema.json";

        private readonly IDictionary<string, Type> _itemFields;
        private const string VirtualAssetsField = "virtual:assets";

        internal VirtualAssetsStacExtension(IStacObject stacObject)
            : base(JsonSchemaUrl, stacObject)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(VirtualAssetsField, typeof(VirtualAsset));
        }

        /// <summary>
        /// Gets or sets virtual Assets
        /// </summary>
        /// <value>
        /// Virtual Assets
        /// </value>
        public IDictionary<string, VirtualAsset> Assets
        {
            get { return this.StacPropertiesContainer.GetProperty<IDictionary<string, VirtualAsset>>(VirtualAssetsField); }
            set { this.StacPropertiesContainer.SetProperty(VirtualAssetsField, value); this.DeclareStacExtension(); }
        }

        /// <summary>
        /// Gets potential fields and their types
        /// </summary>
        /// <value>
        /// Potential fields and their types
        /// </value>
        public override IDictionary<string, Type> ItemFields => this._itemFields;
    }

    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class VirtualAssetsStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a VirtualAssetsStacExtension class from a STAC item
        /// </summary>
        public static VirtualAssetsStacExtension EoExtension(this StacItem stacItem)
        {
            return new VirtualAssetsStacExtension(stacItem);
        }

        /// <summary>
        /// Initilize a VirtualAssetsStacExtension class from a STAC collection
        /// </summary>
        public static VirtualAssetsStacExtension EoExtension(this StacCollection stacCollection)
        {
            return new VirtualAssetsStacExtension(stacCollection);
        }
    }
}
