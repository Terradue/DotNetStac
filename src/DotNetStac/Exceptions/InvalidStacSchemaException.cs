using System;
using System.Runtime.Serialization;

namespace Stac.Exceptions
{
    [Serializable]
    public class InvalidStacSchemaException : Exception
    {
        public InvalidStacSchemaException()
        {
        }

        public InvalidStacSchemaException(string message) : base(message)
        {
        }

        public InvalidStacSchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStacSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
