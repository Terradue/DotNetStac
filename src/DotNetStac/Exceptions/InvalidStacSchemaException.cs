// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: InvalidStacSchemaException.cs

using System;
using System.Runtime.Serialization;

namespace Stac.Exceptions
{
    /// <summary>
    /// Exception thrown when the STAC schema is invalid
    /// </summary>
    [Serializable]
    public class InvalidStacSchemaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStacSchemaException"/> class.
        /// </summary>
        public InvalidStacSchemaException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStacSchemaException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidStacSchemaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStacSchemaException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidStacSchemaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStacSchemaException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected InvalidStacSchemaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
