// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: Preconditions.cs

using System;

namespace Stac
{
    public static class Preconditions
    {
        public static T CheckNotNull<T>(T value, string argName = null)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(argName);
            }

            return value;
        }
    }
}
