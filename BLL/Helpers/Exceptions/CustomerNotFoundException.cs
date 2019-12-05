﻿using System;

namespace BLL.Helpers.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() : base("Customer not found")
        { }
        public CustomerNotFoundException(string message) : base(message)
        { }
    }
}