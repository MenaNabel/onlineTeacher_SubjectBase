using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Exceptions
{
    [Serializable]
    public class EntityNotFound : Exception
    {
        public EntityNotFound()
        {
        }

        public EntityNotFound(string message) : base(message)
        {
        }

        public EntityNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
