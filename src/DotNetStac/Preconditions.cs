using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stac
{
    public static class Preconditions
    {
        public static T CheckNotNull<T>(T value, string argName = null) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(argName);
            }
            return value;
        }
    }

}
