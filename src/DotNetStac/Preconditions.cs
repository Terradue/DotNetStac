// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: Preconditions.cs

using System;

namespace Stac
{
    /// <summary>
    /// A collection of methods to check preconditions.
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Checks that the specified value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="argName">The name of the argument.</param>
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
