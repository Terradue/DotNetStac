// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: InvalidStacDataException.cs

using System;
using System.Runtime.Serialization;

namespace Stac.Exceptions
{
    [Serializable]
    public class InvalidStacDataException : Exception
    {
        public InvalidStacDataException()
        {
        }

        public InvalidStacDataException(string message)
            : base(message)
        {
        }

        public InvalidStacDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidStacDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
