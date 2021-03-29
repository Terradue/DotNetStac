using System;
using System.Runtime.Serialization;

namespace Stac
{
    [Serializable]
    internal class InvalidStacDataException : Exception
    {
        public InvalidStacDataException()
        {
        }

        public InvalidStacDataException(string message) : base(message)
        {
        }

        public InvalidStacDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStacDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}