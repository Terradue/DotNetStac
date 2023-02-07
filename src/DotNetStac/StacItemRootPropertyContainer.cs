// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacItemRootPropertyContainer.cs

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stac
{
    internal class StacItemRootPropertyContainer : IStacPropertiesContainer
    {
        private readonly StacItem stacItem;
        private IDictionary<string, object> properties;

        public StacItemRootPropertyContainer(StacItem stacItem)
        {
            this.stacItem = stacItem;
            properties = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Properties { get => properties; internal set => properties = value; }

        [ExcludeFromCodeCoverage]
        public IStacObject StacObjectContainer => stacItem;
    }
}
