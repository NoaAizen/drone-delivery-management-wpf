using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BO
{
    /// <summary>
    /// חריגת כבר קיים
    /// </summary>
    [Serializable]
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException() : base() { }
        public AlreadyExistException(string message) : base(message) { }
        public AlreadyExistException(string message, Exception inner) : base(message, inner) { }
        protected AlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// חריגה לא קיימת
    /// </summary>
    [Serializable]
    public class DoesntExistException : Exception
    {
        public DoesntExistException() : base() { }
        public DoesntExistException(string message) : base(message) { }
        public DoesntExistException(string message, Exception inner) : base(message, inner) { }
        protected DoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
    /// <summary>
    /// חריגת בעייה בביצוע פעולה
    /// </summary>
    [Serializable]
    public class ActionProblemException : Exception
    {
        public ActionProblemException() : base() { }
        public ActionProblemException(string message) : base(message) { }
        public ActionProblemException(string message, Exception inner) : base(message, inner) { }
        protected ActionProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
