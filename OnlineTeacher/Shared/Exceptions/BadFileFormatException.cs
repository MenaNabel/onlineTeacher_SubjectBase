using System;
using System.Runtime.Serialization;

namespace OnlineTeacher.Shared.Exceptions
{
    [Serializable]
    internal class BadFileFormatException : Exception
    {
        public BadFileFormatException()
        {
        }

        public BadFileFormatException(string message) : base(message)
        {
        }

        public BadFileFormatException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        protected BadFileFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}