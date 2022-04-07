using System;
using System.Collections.Generic;
using Stac.Collection;

namespace Stac.Extensions
{
    public interface IStacAssetExtension
    {
        StacAsset StacAsset { get; }
    }
}
