// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: IStacPropertiesContainer.cs

using System.Collections.Generic;

namespace Stac
{
    /// <summary>
    /// Common interface for all Stac objects
    /// </summary>
    public interface IStacPropertiesContainer
    {
        IDictionary<string, object> Properties { get; }

        IStacObject StacObjectContainer { get; }

    }
}
