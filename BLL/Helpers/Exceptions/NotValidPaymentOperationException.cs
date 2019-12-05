using System;

namespace BLL.Helpers.Exceptions
{
    public class NotValidPaymentOperationException : Exception
    {
        public NotValidPaymentOperationException() : base("Payment operation is not valid")
        { }
        public NotValidPaymentOperationException(string message) : base(message)
        { }
    }
}