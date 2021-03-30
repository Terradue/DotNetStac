using System;
using System.Runtime.Serialization;

namespace Stac.Exceptions
{
    [Serializable]
    public class ExtensionNotAssignedException : Exception
    {
        private string v;
        private string id;

        public ExtensionNotAssignedException()
        {
        }

        public ExtensionNotAssignedException(string message) : base(message)
        {
        }

        public ExtensionNotAssignedException(string v, string id)
        {
            this.v = v;
            this.id = id;
        }

        public ExtensionNotAssignedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExtensionNotAssignedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}