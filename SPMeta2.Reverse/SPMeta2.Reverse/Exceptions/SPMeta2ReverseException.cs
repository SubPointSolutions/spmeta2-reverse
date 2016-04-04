using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Exceptions;

namespace SPMeta2.Reverse.Exceptions
{
    [Serializable]
    public class SPMeta2ReverseException : SPMeta2Exception
    {
        #region constructors

        public SPMeta2ReverseException() { }
        public SPMeta2ReverseException(string message) : base(message) { }
        public SPMeta2ReverseException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2ReverseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
