// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: IStacExtension.cs

using System.Collections.Generic;

namespace Stac.Extensions
{
    /// <summary>
    /// Interface for Stac extensions
    /// </summary>
    public interface IStacExtension
    {
        /// <summary>
        /// Gets the Stac Extension identifier.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Gets the Stac Extension summary functions.
        /// </summary>
        /// <returns>The Stac Extension summary functions.</returns>
        IDictionary<string, ISummaryFunction> GetSummaryFunctions();
    }
}
