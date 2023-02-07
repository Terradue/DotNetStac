// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: IStacExtension.cs

using System.Collections.Generic;

namespace Stac.Extensions
{
    public interface IStacExtension
    {
        string Identifier { get; }

        bool IsDeclared { get; }

        IDictionary<string, ISummaryFunction> GetSummaryFunctions();
    }
}
