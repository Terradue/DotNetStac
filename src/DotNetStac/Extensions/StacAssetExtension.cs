using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Exceptions;

namespace Stac.Extensions
{
    public abstract class StacAssetExtension : IStacExtension
    {
        private readonly string identifier;

        public StacAssetExtension(string identifier, StacAsset stacAsset)
        {
            this.identifier = identifier;
            StacAsset = stacAsset;
        }

        public virtual string Identifier => identifier;

        public StacAsset StacAsset { get; private set; }

    }
}