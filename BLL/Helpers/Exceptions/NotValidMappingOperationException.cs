using System;

namespace BLL.Helpers.Exceptions
{
    public class NotValidMappingOperationException : Exception
    {
        public NotValidMappingOperationException() : base("Mapping operation is not valid")
        { }
        public NotValidMappingOperationException(string message) : base(message)
        { }
    }
}