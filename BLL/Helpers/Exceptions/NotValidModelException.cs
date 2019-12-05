using System;

namespace BLL.Helpers.Exceptions
{
    public class NotValidModelException : Exception
    {
        public NotValidModelException() : base("Model is not valid")
        { }
        public NotValidModelException(string message) : base(message)
        { }
    }
}