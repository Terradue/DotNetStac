// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: VirtualAssetsStacExtension.cs

using System;
using System.Collections.Generic;

namespace Stac.Extensions.VirtualAssets
{
    /// <summary>
    /// Helper class to access the fields deined by the <seealso href="https://github.com/stac-extensions/virtual-assets">virtual assets extension</seealso>
    /// </summary>
    public class VirtualAssetsStacExtension : StacPropertiesContainerExtension, IStacExtension
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string JsonSchemaUrl = "https://stac-extensions.github.io/virtual-assets/v1.0.0/schema.json";

        public const string VirtualAssetsField = "virtual:assets";

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private readonly IDictionary<string, Type> _itemFields;

        internal VirtualAssetsStacExtension(IStacObject stacObject)
            : base(JsonSchemaUrl, stacObject)
        {
            this._itemFields = new Dictionary<string, Type>();
            this._itemFields.Add(VirtualAssetsField, typeof(VirtualAsset));
        }

        /// <summary>
        /// Gets or sets virtual Assets
        /// </summary>
        public IDictionary<string, VirtualAsset> Assets
        {
            get
            {
                return this.StacPropertiesContainer.GetProperty<IDictionary<string, VirtualAsset>>(VirtualAssetsField);
            }

            set
            {
                this.StacPropertiesContainer.SetProperty(VirtualAssetsField, value);
                this.DeclareStacExtension();
            }
        }

        /// <inheritdoc/>
        public override IDictionary<string, Type> ItemFields => this._itemFields;
    }
}
