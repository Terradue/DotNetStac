// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacItemRootPropertyContainer.cs

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stac
{
    internal class StacItemRootPropertyContainer : IStacPropertiesContainer
    {
        private readonly StacItem _stacItem;
        private IDictionary<string, object> _properties;

        public StacItemRootPropertyContainer(StacItem stacItem)
        {
            this._stacItem = stacItem;
            this._properties = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Properties { get => this._properties; internal set => this._properties = value; }

        [ExcludeFromCodeCoverage]
        public IStacObject StacObjectContainer => this._stacItem;
    }
}
